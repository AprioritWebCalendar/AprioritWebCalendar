using System;
using System.Collections.Generic;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Business.Recurrences
{
    public class RecurrenceCalculator
    {
        private readonly Event _event;
        private readonly RecurrenceRule _rule;

        public RecurrenceCalculator(Event @event)
        {
            if (@event.Period == null)
                throw new NullReferenceException(nameof(Event.Period));

            if (@event.Period.Type == PeriodType.Custom && @event.Period.Cycle == null)
                throw new NullReferenceException(nameof(Period.Cycle));

            _event = @event;
            _rule = new RecurrenceRule(_event.Period);
        }

        public IEnumerable<Event> GetRecurrences()
        {
            var list = new List<Event>();
            
            foreach (var date in _rule.GetDates())
            {
                var ev = _event.Clone() as Event;
                ev.StartDate = date;
                ev.EndDate = date;

                if (!ev.IsAllDay)
                {
                    if (ev.StartTime.Value > ev.EndTime.Value)
                        ev.EndDate = ev.EndDate.Value.AddDays(1);
                }

                if (ev.StartTime != null && ev.EndTime != null)
                {
                    ev.StartDate.Value.Add(ev.StartTime.Value);
                    ev.EndDate.Value.Add(ev.EndTime.Value);
                }

                list.Add(ev);
            }

            return list;
        }
    }
}
