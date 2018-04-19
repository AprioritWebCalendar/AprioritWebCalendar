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

namespace AprioritWebCalendar.Web.Controllers
{
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
    }
}