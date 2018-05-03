using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ical.Net;
using Ical.Net.DataTypes;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Infrastructure.Enums;
using AprioritWebCalendar.Data.Interfaces;
using DataCalendar = AprioritWebCalendar.Data.Models.Calendar;
using DomainCalendar = AprioritWebCalendar.Business.DomainModels.Calendar;

namespace AprioritWebCalendar.Business.Services
{
    // TODO: "RemindBefore".
    public class CalendarImportService : ICalendarImportService
    {
        private readonly IRepository<DataCalendar> _calendarRepository;
        private readonly IMapper _mapper;

        public CalendarImportService(IRepository<DataCalendar> calendarRepository)
        {
            _calendarRepository = calendarRepository;
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new CalendarImportMapperProfile())).CreateMapper();
        }

        public async Task<DomainCalendar> SaveCalendarAsync(DomainCalendar calendar)
        {
            var dataCalendar = _mapper.Map<DataCalendar>(calendar);
            dataCalendar.OwnerId = calendar.Owner.Id;

            foreach (var evCal in calendar.Events)
            {
                // Crutch.

                evCal.Calendar = calendar;
                var ev = _mapper.Map<Data.Models.EventCalendar>(evCal);
                
                ev.Event.OwnerId = calendar.Owner.Id;

                if (ev.Event.IsAllDay && ev.Event.Period == null)
                    ev.Event.EndDate = ev.Event.EndDate.Value.AddDays(-1);

                dataCalendar.EventCalendars.Add(ev);
            }

            dataCalendar = await _calendarRepository.CreateAsync(dataCalendar);
            await _calendarRepository.SaveAsync();

            calendar.Id = dataCalendar.Id;
            return calendar;
        }

        public DomainCalendar ConvertIntoDomain(Calendar iCalendar)
        {
            var domainCalendar = new DomainCalendar();
            var domainEvents = new List<DomainModels.Event>();

            var iCalTimezone = TimeZoneInfo.FindSystemTimeZoneById(iCalendar.TimeZones.FirstOrDefault()?.TzId)
                ?? TimeZoneInfo.Utc;

            foreach (var iEvent in iCalendar.Events)
            {
                var dEvent = new DomainModels.Event()
                {
                    Name = iEvent.Summary,
                    Description = iEvent.Description,
                    IsAllDay = iEvent.IsAllDay,
                    IsPrivate = false
                };

                if (iEvent.GeographicLocation != null)
                {
                    dEvent.Location = new DomainModels.Location()
                    {
                        Description = iEvent.Location,
                        Longitude = iEvent.GeographicLocation.Longitude,
                        Lattitude = iEvent.GeographicLocation.Latitude
                    };
                }

                if (iEvent.RecurrenceRules.Any())
                {
                    var rule = iEvent.RecurrenceRules.First();
                    dEvent.Period = _GetPeriod(rule);
                    dEvent.Period.PeriodStart = iEvent.DtStart.Date;

                    if (!iEvent.IsAllDay)
                    {
                        //dEvent.StartTime = TimeZoneInfo.ConvertTimeToUtc(iEvent.DtStart.Value, iCalTimezone).TimeOfDay;
                        //dEvent.EndTime = TimeZoneInfo.ConvertTimeToUtc(iEvent.DtEnd.Value, iCalTimezone).TimeOfDay;
                        dEvent.StartTime = iEvent.DtStart.Value.TimeOfDay;
                        dEvent.EndTime = iEvent.DtEnd.Value.TimeOfDay;
                    }
                }
                else
                {
                    if (iEvent.IsAllDay)
                    {
                        dEvent.StartDate = iEvent.DtStart.Value;
                        dEvent.EndDate = iEvent.DtEnd.Value;
                    }
                    else
                    {
                        //var utcStart = TimeZoneInfo.ConvertTimeToUtc(iEvent.DtStart.Value, iCalTimezone);
                        //var utcEnd = TimeZoneInfo.ConvertTimeToUtc(iEvent.DtEnd.Value, iCalTimezone);

                        var utcStart = iEvent.DtStart.Value;
                        var utcEnd = iEvent.DtEnd.Value;

                        dEvent.StartDate = utcStart.Date;
                        dEvent.EndDate = utcEnd.Date;
                        dEvent.StartTime = utcStart.TimeOfDay;
                        dEvent.EndTime = utcEnd.TimeOfDay;
                    }
                }

                domainEvents.Add(dEvent);
            }

            if (domainEvents.Any())
            {
                domainCalendar.Events = domainEvents
                    .Select(e => new DomainModels.EventCalendar { Event = e, IsReadOnly = false });
            }

            return domainCalendar;
        }

        public Calendar DeserializeCalendar(string icsString)
        {
            return Calendar.Load(icsString);
        }

        private DomainModels.Period _GetPeriod(RecurrencePattern recurrencePattern)
        {
            var period = new DomainModels.Period()
            {
                Cycle = recurrencePattern.Frequency == FrequencyType.Daily ? recurrencePattern.Interval : 1,
                PeriodEnd = recurrencePattern.Until.Date
            };

            switch (recurrencePattern.Frequency)
            {
                case FrequencyType.Yearly:
                    period.Type = PeriodType.Yearly;
                    break;

                case FrequencyType.Monthly:
                    period.Type = PeriodType.Monthly;
                    break;

                case FrequencyType.Weekly:
                    period.Type = PeriodType.Weekly;
                    break;

                case FrequencyType.Daily:
                    period.Type = PeriodType.Custom;
                    break;

                default:
                    throw new NotSupportedException();
            }

            return period;
        }
    }
}
