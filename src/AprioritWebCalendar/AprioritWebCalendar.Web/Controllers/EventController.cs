using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Business.Interfaces;
using AutoMapper;
using AprioritWebCalendar.ViewModel.Calendar;
using AprioritWebCalendar.ViewModel.Event;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService,
            IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetEvents(int calendarId, DateTime start, DateTime end)
        {
            var events = await _eventService.GetEventsAsync(this.GetUserId(), start, end);
            var viewModels = _mapper.Map<IEnumerable<EventViewModel>>(events);
            return this.OkOrNoContent(viewModels);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var calendar = _mapper.Map<EventViewModel>(await _eventService.GetEventByIdAsync(id));
            return Ok(calendar);
        }
        [HttpPost, ValidateApiModelFilter]
        public async Task<IActionResult> Create([FromBody]EventViewModel model)
        {
            // TODO: Validate an event view model 
            //(await _calendarValidator.ValidateCreateAsync(model, this.GetUserId())).ToModelState(ModelState);

            // if (!ModelState.IsValid)                return BadRequest(ModelState.ToStringEnumerable());


            var domainEvent = _mapper.Map<Event>(model);
            var id = await _eventService.CreateEventAsync(domainEvent, model.CalendarId);
            return Ok(new { Id = id });
        }
    }
}