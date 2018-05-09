using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Business.Recurrences;
using EventCalendar = AprioritWebCalendar.Data.Models.EventCalendar;
using DomainEventCalendar = AprioritWebCalendar.Business.DomainModels.EventCalendar;

namespace AprioritWebCalendar.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<EventCalendar> _eventCalendarRepository;
        private readonly IMapper _mapper;

        public NotificationService(IRepository<EventCalendar> eventCalendarRepository, IMapper mapper)
        {
            _eventCalendarRepository = eventCalendarRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventUser>> GetEventsToNotifyAsync(DateTime dateTime)
        {
            dateTime = dateTime.AddSeconds(-dateTime.Second);
            var dateDate = dateTime.Date;

            Expression<Func<EventCalendar, bool>> filter = e => e.Event.RemindBefore != null &&
                ((e.Event.Period != null && dateDate >= e.Event.Period.PeriodStart && dateDate <= e.Event.Period.PeriodEnd)
                || (e.Event.Period == null && !e.Event.IsAllDay && EFFunctions.DateDiffMinute(e.Event.StartDate.Value.AddMinutes(e.Event.StartTime.Value.TotalMinutes).AddMinutes(-e.Event.RemindBefore.Value), dateTime) == 0)
                || (e.Event.Period == null && e.Event.IsAllDay && EFFunctions.DateDiffHour(dateTime, e.Event.StartDate.Value.AddMinutes(-e.Event.RemindBefore.Value)) <= 24));

            var eventCalendars = (await _eventCalendarRepository.FindAllIncludingAsync(e => e.Calendar, e => e.Calendar.Owner, e => e.Calendar.SharedUsers,
                        e => e.Event, e => e.Event.Period))
                .Where(filter)
                .AsNoTracking()
                .ToList();
            
            if (!eventCalendars.Any())
                return null;

            var domainEventCalendars = _mapper.Map<List<DomainEventCalendar>>(eventCalendars);

            domainEventCalendars = CalculateRecurrences(domainEventCalendars)
                .Where(e => (!e.Event.IsAllDay && EFFunctions.DateDiffMinute(dateTime, e.Event.StartDate.Value.Add(e.Event.StartTime.Value).AddMinutes(-e.Event.RemindBefore.Value)) == 0)
                             || (e.Event.IsAllDay && EFFunctions.DateDiffMinute(e.Calendar.Owner.TimeZone.ConvertFromUtc(dateTime), e.Event.StartDate.Value.AddMinutes(-e.Event.RemindBefore.Value)) == 0))
                .ToList();

            if (!domainEventCalendars.Any())
                return null;

            var eventUsers = domainEventCalendars.SelectMany(e => e.Calendar.SharedUsers.Where(u => u.IsSubscribed).Select(c => new EventUser
            {
                Event = e.Event,
                User = c.User
            })).ToList();

            var owners = domainEventCalendars.Select(e => new EventUser
            {
                Event = e.Event,
                User = e.Calendar.Owner
            });

            eventUsers.AddRange(owners);

            return eventUsers;
        }

        private IEnumerable<DomainEventCalendar> CalculateRecurrences(IEnumerable<DomainEventCalendar> eventCalendars)
        {
            var list = new List<DomainEventCalendar>();

            foreach (var evCal in eventCalendars)
            {
                if (evCal.Event.Period == null)
                {
                    list.Add(evCal);
                }
                else
                {
                    var calc = new RecurrenceCalculator(evCal.Event);

                    list.AddRange(calc.GetRecurrences().Select(ev => new DomainEventCalendar
                    {
                        Event = ev,
                        Calendar = evCal.Calendar
                    }));
                }
            }

            return list;
        }
    }
}
