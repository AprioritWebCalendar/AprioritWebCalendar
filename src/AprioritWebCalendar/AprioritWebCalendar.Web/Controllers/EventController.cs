using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.ViewModel.Calendar;
using AprioritWebCalendar.ViewModel.Event;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Infrastructure.Extensions;

namespace AprioritWebCalendar.Web.Controllers
{
    // TODO: Private events.
    // TODO: Whole days count (for Period).

    [Produces("application/json")]
    [Route("api/Event")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public EventController(
            IEventService eventService,
            ICalendarService calendarService,
            IMapper mapper)
        {
            _eventService = eventService;
            _calendarService = calendarService;
            _mapper = mapper;
        }

        #region GET.

        [HttpGet, ValidateApiModelFilter]
        public async Task<IActionResult> Get(EventsRequestModel model)
        {
            var userId = this.GetUserId();

            if (!await _calendarService.IsOwnerOrSharedWithAsync(model.CalendarsIds, userId))
            {
                return this.BadRequestError("You are requesting a calendar to which you don't have any permissions.");
            }

            var eventsDomain = await _eventService.GetEventsAsync(userId, model.StartDate, model.EndDate, model.CalendarsIds.ToArray());

            if (eventsDomain?.Any() != true)
                return NoContent();

            return Ok(_mapper.Map<IEnumerable<EventViewModel>>(eventsDomain));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = this.GetUserId();

            if (!await _eventService.IsOwnerOrInvitedAsync(id, userId))
            {
                return this.BadRequestError("You don't have any permissions to this event.");
            }

            // TODO: Add Color(?), IsReadOnly and CalendarId.
            var eventDomain = await _eventService.GetEventByIdAsync(id, "Period", "Owner");
            return Ok(_mapper.Map<EventViewModel>(eventDomain));
        }

        [HttpGet("{id}/Users")]
        public async Task<IActionResult> GetInvitedUsers(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region POST.

        [HttpPost, ValidateApiModelFilter]
        public async Task<IActionResult> Create([FromBody]EventRequestModel model)
        {
            var userId = this.GetUserId();

            if (!await _calendarService.CanEditAsync(model.CalendarId, userId))
            {
                return this.BadRequestError("You don't have any permissions to create event in that calendar.");
            }

            var eventDomain = _mapper.Map<Event>(model);
            var id = await _eventService.CreateEventAsync(eventDomain, userId);
            return Ok(new { Id = id });
        }

        #endregion

        #region PUT.

        [HttpPut("{id}"), ValidateApiModelFilter]
        public async Task<IActionResult> Update(int id, [FromBody]EventRequestModel model)
        {
            if (!await _eventService.CanEditAsync(id, this.GetUserId()))
                return this.BadRequestError("You don't have any permissions to edit that event.");

            var eventDomain = await _eventService.GetEventByIdAsync(id, "Period");

            // There might be some errors.
            eventDomain = _mapper.Map(model, eventDomain);
            eventDomain.Id = id;
            await _eventService.UpdateEventAsync(eventDomain);

            return Ok();
        }

        [HttpPut("{id}/Move"), ValidateApiModelFilter]
        public async Task<IActionResult> Move(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/Invite")]
        public async Task<IActionResult> InviteUser(int id, [FromBody]InviteViewModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/Accept")]
        public async Task<IActionResult> AcceptInvitation(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectInvitation(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/ReadOnly/{userId}")]
        public async Task<IActionResult> SetEventReadOnlyState(int id, int userId, [FromBody]bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/Invitation/ReadOnly/{userId}")]
        public async Task<IActionResult> SetInvitationReadOnlyState(int id, int userId, [FromBody]bool isReadOnly)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DELETE.

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}/Invitation/{userId}")]
        public async Task<IActionResult> DeleteInvitation(int id, [FromBody]int userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}