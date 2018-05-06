using System;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class Event : IComparable<Event>, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Location Location { get; set; }
        public User Owner { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        
        public bool IsAllDay { get; set; }
        public int? RemindBefore { get; set; }
        public bool IsPrivate { get; set; } = true;

        public Period Period { get; set; }

        public int CalendarId { get; set; }
        public string Color { get; set; }
        public bool IsReadOnly { get; set; }

        public object Clone()
        {
            return new Event
            {
                Id = Id,
                Name = Name,
                Description = Description,

                Location = Location,
                Owner = Owner,

                StartDate = StartDate,
                EndDate = EndDate,
                StartTime = StartTime,
                EndTime = EndTime,

                IsAllDay = IsAllDay,
                RemindBefore = RemindBefore,
                IsPrivate = IsPrivate,

                Period = Period,

                CalendarId = CalendarId,
                Color = Color,
                IsReadOnly = IsReadOnly
            };
        }

        public int CompareTo(Event other)
        {
            if (Period == null)
            {
                return StartDate.Value.CompareTo(other.StartDate ?? other.Period.PeriodStart);
            }
            else
            {
                return Period.PeriodStart.CompareTo(other.StartDate ?? other.Period.PeriodStart);
            }
        }

        public bool IsOld(DateTime dateTime)
        {
            DateTime ending;

            if (Period == null)
            {
                ending = IsAllDay ? EndDate.Value.AddDays(1)
                    : EndDate.Value.Add(EndTime.Value);
            }
            else
            {
                ending = IsAllDay ? Period.PeriodEnd.AddDays(1)
                    : Period.PeriodEnd.Add(EndTime.Value);
            }

            return dateTime >= ending;
        }
    }
}
