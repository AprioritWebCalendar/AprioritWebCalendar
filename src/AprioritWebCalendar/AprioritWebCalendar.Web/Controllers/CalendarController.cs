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

namespace AprioritWebCalendar.Web.Controllers
{
    // TODO: Subscribe/Unsubscribe own calendar.

    [Produces("application/json")]
    [Route("api/Calendar")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CalendarController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;
        private readonly ICalendarValidator _calendarValidator;

        public CalendarController(
            IMapper mapper, 
            ICalendarService calendarService,
            ICalendarValidator calendarValidator)
        {
            _mapper = mapper;
            _calendarService = calendarService;
            _calendarValidator = calendarValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCalendars(bool onlyOwn = false)
        {
            var userId = this.GetUserId();

            var calendars = await _calendarService.GetCalendarsAsync(userId, onlyOwn);
            var viewModels = _mapper.MapToCalendarViewModel(calendars.ToList(), userId);

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
            // TODO: Replace for custom exception.

            if (!await _calendarService.CanEditAsync(id, this.GetUserId()))
                throw new ArgumentException();

            var calendar = await _calendarService.GetCalendarByIdAsync(id);

            // Owner can't have any calendars with the same names.
            // If calendar is edited by another user that can permissions, we need to validate it too.
            (await _calendarValidator.ValidateUpdateAsync(id, model, calendar.Owner.Id)).ToModelState(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToStringEnumerable());
            
            calendar.Id = id;
            calendar.Name = model.Name;
            calendar.Description = model.Description;
            calendar.Color = model.Color;

            await _calendarService.UpdateCalendarAsync(calendar);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // TODO: Replace for custom exception.

            if (!await _calendarService.IsOwnerAsync(id, this.GetUserId()))
                throw new ArgumentException();

            // TODO: Check for existing events, etc..?

            await _calendarService.DeleteCalendarAsync(id);
            return Ok();
        }

        [HttpPut("{id}/Share"), ValidateApiModelFilter]
        public async Task<IActionResult> Share(int id, [FromBody]CalendarShareRequest model)
        {
            await _calendarService.ShareCalendarAsync(id, model.UserId.Value, model.IsReadOnly);
            return Ok();
        }

        [HttpPut("{id}/RemoveSharing")]
        public async Task<IActionResult> RemoveSharing(int id, [FromBody]int userId)
        {
            if (userId == this.GetUserId())
                return this.BadRequestError("You can't remove sharing for your calendar.");

            // TODO: Replace for custom exception.
            
            if (!await _calendarService.IsOwnerAsync(id, this.GetUserId()))
                throw new ArgumentException();

            await _calendarService.RemoveSharingAsync(id, userId);
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
    }
}
