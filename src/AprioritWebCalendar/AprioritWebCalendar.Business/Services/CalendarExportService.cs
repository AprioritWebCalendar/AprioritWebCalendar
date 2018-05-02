using System;
using System.Collections.Generic;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Infrastructure.Enums;
using DomainCalendar = AprioritWebCalendar.Business.DomainModels.Calendar;

namespace AprioritWebCalendar.Business.Services
{
    public class CalendarExportService : ICalendarExportService
    {
        public Calendar ExportCalendar(DomainCalendar calendar)
        {
            var iCalendar = new Calendar();
            iCalendar.TimeZones.Add(new VTimeZone(TimeZoneInfo.Utc.Id));

            foreach (var e in calendar.Events)
            {
                var iEvent = new CalendarEvent
                {
                    Summary = e.Event.Name,
                    Description = e.Event.Description,
                    IsAllDay = e.Event.IsAllDay
                };

                if (e.Event.Location != null)
                {
                    iEvent.GeographicLocation = new GeographicLocation(e.Event.Location.Lattitude.Value, e.Event.Location.Longitude.Value);
                    iEvent.Location = e.Event.Location.Description;
                }

                if (e.Event.Period == null)
                {
                    if (e.Event.IsAllDay)
                    {
                        iEvent.DtStart = new CalDateTime(e.Event.StartDate.Value);
                        iEvent.DtEnd = new CalDateTime(e.Event.EndDate.Value.AddDays(1).AddMinutes(-1));
                    }
                    else
                    {
                        iEvent.DtStart = new CalDateTime(e.Event.StartDate.Value.Add(e.Event.StartTime.Value));
                        iEvent.DtEnd = new CalDateTime(e.Event.EndDate.Value.Add(e.Event.EndTime.Value));
                    }
                }
                else
                {
                    if (e.Event.IsAllDay)
                    {
                        iEvent.DtStart = new CalDateTime(e.Event.Period.PeriodStart);
                        iEvent.DtEnd = new CalDateTime(e.Event.Period.PeriodStart.AddDays(1).AddMinutes(-1));
                    }
                    else
                    {
                        iEvent.DtStart = new CalDateTime(e.Event.Period.PeriodStart.Add(e.Event.StartTime.Value));
                        iEvent.DtEnd = new CalDateTime(e.Event.Period.PeriodEnd.Add(e.Event.EndTime.Value));
                    }

                    iEvent.RecurrenceRules = new List<RecurrencePattern> { _GetRecurrencePattern(e.Event) };
                }

                iCalendar.Events.Add(iEvent);
            }

            return iCalendar;
        }

        public string SerializeCalendar(Calendar iCalendar)
        {
            return new CalendarSerializer(iCalendar).SerializeToString();
        }

        private RecurrencePattern _GetRecurrencePattern(DomainModels.Event ev)
        {
            var rule = new RecurrencePattern
            {
                Interval = ev.Period.Type == PeriodType.Custom ? ev.Period.Cycle.Value : 1,
                Until = ev.Period.PeriodEnd
            };

            switch (ev.Period.Type)
            {
                case PeriodType.Yearly:
                    rule.Frequency = FrequencyType.Yearly;
                    break;

                case PeriodType.Monthly:
                    rule.Frequency = FrequencyType.Monthly;
                    break;

                case PeriodType.Weekly:
                    rule.Frequency = FrequencyType.Weekly;
                    break;

                case PeriodType.Custom:
                    rule.Frequency = FrequencyType.Daily;
                    break;
            }

            return rule;
        }
    }
}
