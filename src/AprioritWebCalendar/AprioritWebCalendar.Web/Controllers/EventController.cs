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
    // TODO: Whole days count (for Period).
    // TODO: Separated controller for invitations.

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

        [HttpGet("Search/{text}/{take?}")]
        public async Task<IActionResult> Get(string text, int take = 5)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 3)
                return this.BadRequestError("The text to search is required and must be at least than 3 symbols.");

            var events = await _eventService.GetEventsByNameAsync(text, this.GetUserId(), take);

            if (events?.Any() != true)
                return NoContent();

            var viewEvents = _mapper.Map<IEnumerable<EventSearchResultModel>>(events);
            return Ok(viewEvents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = this.GetUserId();

            if (!await _eventService.IsOwnerOrInvitedAsync(id, userId))
            {
                return this.BadRequestError("You don't have any permissions to this event.");
            }

            var eventDomain = await _eventService.GetEventByIdAsync(id, userId);
            return Ok(_mapper.Map<EventViewModel>(eventDomain));
        }

        [HttpGet("{id}/Users")]
        public async Task<IActionResult> GetInvitedUsers(int id)
        {
            if (!await _eventService.IsOwnerAsync(id, this.GetUserId()))
            {
                return this.BadRequestError("Only owner can see invited users.");
            }

            var users = await _eventService.GetInvitedUsersAsync(id);
            return this.OkOrNoContent(users);
        }

        [HttpGet("Invitation/Incoming")]
        public async Task<IActionResult> GetIncomingInvitations()
        {
            return this.OkOrNoContent(await _eventService.GetIncomingInvitationsAsync(this.GetUserId()));
        }

        [HttpGet("Invitation/Outcoming")]
        public async Task<IActionResult> GetOutcomingInvitations()
        {
            return this.OkOrNoContent(await _eventService.GetOutcomingInvitationsAsync(this.GetUserId()));
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
            
            eventDomain = _mapper.Map(model, eventDomain);
            eventDomain.Id = id;
            await _eventService.UpdateEventAsync(eventDomain);

            return Ok();
        }

        [HttpPut("{id}/Move"), ValidateApiModelFilter]
        public async Task<IActionResult> Move(int id, [FromBody]EventMoveRequest model)
        {
            var userId = this.GetUserId();

            if (!await _calendarService.IsOwnerAsync(model.OldCalendar.Value, userId)
                || !await _calendarService.IsOwnerAsync(model.NewCalendar.Value, userId))
            {
                return this.BadRequestError("You are not owner of one of these calendars.");
            }

            await _eventService.MoveEventAsync(id, model.OldCalendar.Value, model.NewCalendar.Value);
            return Ok();
        }

        [HttpPut("{id}/Invite")]
        public async Task<IActionResult> InviteUser(int id, [FromBody]InviteViewModel model)
        {
            var userId = this.GetUserId();

            if (model.UserId == userId)
                return this.BadRequestError("You can't invite yourself.");

            if (!await _eventService.IsOwnerAsync(id, userId))
                return this.BadRequestError("Only owner can invite users.");

            if (await _eventService.IsPrivateAsync(id))
                return this.BadRequestError("You can't invite users on private event.");

            await _eventService.InviteUserAsync(id, model.UserId, userId, model.IsReadOnly);
            return Ok();
        }

        [HttpPut("{id}/Accept")]
        public async Task<IActionResult> AcceptInvitation(int id)
        {
            await _eventService.AcceptInvatationAsync(id, this.GetUserId());
            return Ok();
        }

        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectInvitation(int id)
        {
            await _eventService.RejectInvitationAsync(id, this.GetUserId());
            return Ok();
        }

        [HttpPut("{id}/ReadOnly/{userId}")]
        public async Task<IActionResult> SetEventReadOnlyState(int id, int userId, [FromBody]bool isReadOnly)
        {
            if (!await _eventService.IsOwnerAsync(id, this.GetUserId()))
            {
                return this.BadRequestError("Only owner can change read-only state.");
            }

            await _eventService.UpdateEventReadOnlyStateAsync(id, userId, isReadOnly);
            return Ok();
        }

        [HttpPut("{id}/Invitation/ReadOnly/{userId}")]
        public async Task<IActionResult> SetInvitationReadOnlyState(int id, int userId, [FromBody]bool isReadOnly)
        {
            if (!await _eventService.IsOwnerAsync(id, this.GetUserId()))
            {
                return this.BadRequestError("Only owner can change read-only state.");
            }

            await _eventService.UpdateInvitationReadOnlyAsync(id, userId, isReadOnly);
            return Ok();
        }

        #endregion

        #region DELETE.

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!await _eventService.IsOwnerAsync(id, this.GetUserId()))
                return this.BadRequestError("Only owner can delete this event.");

            await _eventService.DeleteEventAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/Invited/{userId}")]
        public async Task<IActionResult> DeleteInvitedUser(int id, int userId)
        {
            var currentUserId = this.GetUserId();
            
            if (userId == currentUserId || await _eventService.IsOwnerAsync(id, currentUserId))
            {
                await _eventService.DeleteIntvitedUserAsync(id, userId);
                return Ok();

            }
            return this.BadRequestError("Only owner can delete invited user.");
        }

        [HttpDelete("{id}/Invitation/{userId}")]
        public async Task<IActionResult> DeleteInvitation(int id, int userId)
        {
            if (!await _eventService.IsOwnerAsync(id, this.GetUserId()))
                return this.BadRequestError("Only owner can delete invitation.");

            await _eventService.RejectInvitationAsync(id, userId);
            return Ok();
        }

        #endregion
    }
}