using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AprioritWebCalendar.Business.Interfaces;
using System.Threading.Tasks;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AutoMapper;
using AprioritWebCalendar.Infrastructure.Exceptions;
using DomainEvent = AprioritWebCalendar.Business.DomainModels.Event;
using DomainEventCalendar = AprioritWebCalendar.Business.DomainModels.EventCalendar;
using DomainUser = AprioritWebCalendar.Business.DomainModels.User;

namespace AprioritWebCalendar.Business.Services
{
    public class EventService : IEventService
    {

        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventCalendar> _eventCalendarRepository;
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;

        public EventService(IRepository<Event> eventRepository,
            IRepository<EventCalendar> eventCalendarRepository,
            ICalendarService calendarService,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _eventCalendarRepository = eventCalendarRepository;
            _calendarService = calendarService;
            _mapper = mapper;
        }
        public async Task<DomainEvent> GetEventByIdAsync(int id)
        {
            var resEvent = (await _eventRepository.FindAllAsync(c => c.Id == id)).FirstOrDefault();

            if (resEvent == null)
                throw new NotFoundException();

            return _mapper.Map<DomainEvent>(resEvent);
        }
        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(int calendarId, DateTime start, DateTime end)
        {
            var eventsCalendar = (await _eventCalendarRepository.FindAllAsync(c => c.Calendar.Id == calendarId));
            IEnumerable<DomainEvent> result = null;
            foreach(var eventCalendar in eventsCalendar)
            {
                if(result == null)
                    result = _mapper.Map<IEnumerable<DomainEvent>>(await _eventRepository.FindAllAsync((c => c.Id == eventCalendar.Event.Id 
                    && c.StartDate >= start &&c.EndDate <= end)));
                else result.Concat(_mapper.Map<IEnumerable<DomainEvent>>(await _eventRepository.FindAllAsync(
                    (c => c.Id == eventCalendar.Event.Id && c.StartDate >= start && c.EndDate <= end))));
            }

            if (result == null)
                throw new NotFoundException();

            return result;
        }
        public async Task<int> CreateEventAsync(DomainEvent domainEvent, int calendarId)
        {
            var calendar = _mapper.Map<Calendar>(await _calendarService.GetCalendarByIdAsync(calendarId));
            var dataEvent = _mapper.Map<Event>(domainEvent);
            if (dataEvent.Owner == null || dataEvent.OwnerId == 0)
            {
                dataEvent.Owner = calendar.Owner;
                dataEvent.OwnerId = calendar.OwnerId;
            }


            dataEvent = await _eventRepository.CreateAsync(dataEvent);
            await _eventRepository.SaveAsync();

            var eventCalendar = new EventCalendar() {EventId = dataEvent.Id, CalendarId = calendar.Id,
            Event = dataEvent, Calendar = calendar, IsReadOnly = false};
            await _eventCalendarRepository.CreateAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();

            return dataEvent.Id;
        }
    }
}
