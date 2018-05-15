using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.ViewModel.Calendar;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Web.SignalR.Notifications;
using AprioritWebCalendar.Web.SignalR.Calendar;

namespace AprioritWebCalendar.Web.Controllers
{
    // TODO: Subscribe/Unsubscribe own calendar.

    [Produces("application/json")]
    [Route("api/Calendar")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ExceptionHandler]
    public class CalendarController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;
        private readonly ICalendarValidator _calendarValidator;

        private readonly NotificationHubManager _notificationManager;
        private readonly CalendarHubManager _calendarManager;

        public CalendarController(
            IMapper mapper, 
            ICalendarService calendarService,
            ICalendarValidator calendarValidator,
            NotificationHubManager notificationHubManager,
            CalendarHubManager calendarHubManager)
        {
            _mapper = mapper;
            _calendarService = calendarService;
            _calendarValidator = calendarValidator;
            _notificationManager = notificationHubManager;
            _calendarManager = calendarHubManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCalendars(bool onlyOwn = false)
        {
            var userId = this.GetUserId();

            var calendars = await _calendarService.GetCalendarsAsync(userId, onlyOwn);
            var viewModels = _mapper.MapToCalendarViewModel(calendars.ToList(), userId);

            // User mustn't know about "IsDefault" state of caledar of another user.
            foreach (var c in viewModels)
            {
                if (c.Owner.Id != userId)
                    c.IsDefault = null;
            }

            return this.OkOrNoContent(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = this.GetUserId();
            // TODO: Replace for custom exception.

            // A calendar can be got only by owner or a user that the calendar shared with.
            if (!await _calendarService.IsOwnerOrSharedWithAsync(id, userId))
                throw new ArgumentException();

            var domainCalendar = await _calendarService.GetCalendarByIdAsync(id);

            var calendarVM = _mapper.MapToCalendarViewModel(domainCalendar, userId);
            return Ok(calendarVM);
        }

        [HttpGet("{id}/SharedUsers")]
        public async Task<IActionResult> GetSharedUsers(int id)
        {
            // TODO: Replace for custom exception.

            // Only owner can see users that calendar is shared with.
            if (!await _calendarService.IsOwnerAsync(id, this.GetUserId()))
                throw new ArgumentException();

            var usersCalendars = await _calendarService.GetUsersSharedWithAsync(id);
            return this.OkOrNoContent(_mapper.Map<IEnumerable<UserCalendarViewModel>>(usersCalendars));
        }

        [HttpPost, ValidateApiModelFilter]
        public async Task<IActionResult> Create([FromBody]CalendarRequestModel model)
        {
            (await _calendarValidator.ValidateCreateAsync(model, this.GetUserId())).ToModelState(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToStringEnumerable());

            var domainCalendar = _mapper.Map<Calendar>(model);
            var id = await _calendarService.CreateCalendarAsync(domainCalendar, this.GetUserId());
            return Ok(new { Id = id });
        }

        [HttpPut("{id}"), ValidateApiModelFilter]
        public async Task<IActionResult> Update(int id, [FromBody]CalendarRequestModel model)
        {
            var userId = this.GetUserId();

            // TODO: Replace for custom exception.

            if (!await _calendarService.CanEditAsync(id, userId))
                throw new ArgumentException();

            var calendar = await _calendarService.GetCalendarByIdAsync(id, nameof(Calendar.Owner));

            // Owner can't have any calendars with the same names.
            // If calendar is edited by another user that can permissions, we need to validate it too.
            (await _calendarValidator.ValidateUpdateAsync(id, model, calendar.Owner.Id)).ToModelState(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToStringEnumerable());

            var oldName = calendar.Name.Clone() as string;
            
            // TODO: Use AutoMapper instead.
            calendar.Id = id;
            calendar.Name = model.Name;
            calendar.Description = model.Description;
            calendar.Color = model.Color;

            await _calendarService.UpdateCalendarAsync(calendar);

            if (userId != calendar.Owner.Id)
            {
                await _calendarManager.CalendarEditedAsync(calendar.Owner.Id, User.Identity.Name, _mapper.Map<CalendarViewModel>(calendar), oldName);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = User.GetUserId();
            var calendar = await _calendarService.GetCalendarByIdAsync(id, nameof(Calendar.Owner), nameof(Calendar.SharedUsers));

            if (calendar.Owner.Id == currentUserId)
            {
                if (await _calendarService.IsDefaultAsync(id))
                    return this.BadRequestError("You can't delete a default calendar.");

                await _calendarService.DeleteCalendarAsync(id);

                if (calendar.SharedUsers?.Any() == true)
                    await _calendarManager.CalendarDeletedAsync(calendar.SharedUsers.Select(u => u.UserId), calendar.Id, calendar.Name, calendar.Owner.UserName);
            }
            else if (calendar.SharedUsers.Any(u => u.UserId == currentUserId))
            {
                await _calendarService.RemoveSharingAsync(id, currentUserId);
            }
            else
            {
                return this.BadRequestError("You don't have any permissions to this calendar.");
            }

            return Ok();
        }

        [HttpPut("{id}/Share"), ValidateApiModelFilter]
        public async Task<IActionResult> Share(int id, [FromBody]CalendarShareRequest model)
        {
            if (!await _calendarService.IsOwnerAsync(id, User.GetUserId()))
                return this.BadRequestError("Only owner can share this calendar.");

            await _calendarService.ShareCalendarAsync(id, model.UserId.Value, model.IsReadOnly);

            var calendar = await _calendarService.GetCalendarByIdAsync(id, nameof(Calendar.Owner));

            var vmCalendar = _mapper.Map<CalendarViewModel>(calendar);
            vmCalendar.IsReadOnly = model.IsReadOnly;
            vmCalendar.IsSubscribed = true;

            await _calendarManager.CalendarSharedAsync(model.UserId.Value, vmCalendar);
            return Ok();
        }

        [HttpPut("{id}/RemoveSharing")]
        public async Task<IActionResult> RemoveSharing(int id, [FromBody]int userId)
        {
            var calendar = await _calendarService.GetCalendarByIdAsync(id, nameof(Calendar.Owner));
            var currentUserId = User.GetUserId();

            if (userId == currentUserId)
                return this.BadRequestError("You can't remove sharing for your calendar.");

            if (calendar.Owner.Id != currentUserId)
                return this.BadRequestError("You don't have any permissions for this action.");

            await _calendarService.RemoveSharingAsync(id, userId);
            
            await _calendarManager.RemovedFromCalendarAsync(userId, calendar.Id, calendar.Name, calendar.Owner.UserName);
            return Ok();
        }

        [HttpPut("{id}/Subscribe")]
        public async Task<IActionResult> Subscribe(int id)
        {
            // It works if calendar is already shared with user.

            await _calendarService.SubscribeCalendarAsync(id, this.GetUserId());
            return Ok();
        }

        [HttpPut("{id}/Unsubscribe")]
        public async Task<IActionResult> Unsubscribe(int id)
        {
            // It works if calendar is already shared with user.

            await _calendarService.UnsunscribeCalendarAsync(id, this.GetUserId());
            return Ok();
        }

        [HttpPut("{id}/ReadOnly/{userId}")]
        public async Task<IActionResult> SetReadOnly(int id, int userId, [FromBody]bool isReadOnly)
        {
            var currentUserId = User.GetUserId();

            if (userId == currentUserId)
                return this.BadRequestError("You can't change this status for yourself.");

            if (!await _calendarService.IsOwnerAsync(id, currentUserId))
                return this.BadRequestError("Only owner can change read-only state.");

            await _calendarService.SetReadOnlyStatusAsync(id, userId, isReadOnly);

            var calendarName = (await _calendarService.GetCalendarByIdAsync(id)).Name;

            await _calendarManager.CalendarReadOnlyChangedAsync(userId, id, calendarName, User.Identity.Name, isReadOnly);
            return Ok();
        }
    }
}
