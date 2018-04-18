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

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }
    }
}