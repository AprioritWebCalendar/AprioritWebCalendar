using System;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class Event
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

        public Period Period { get; set; }

        public int CalendarId { get; set; }
        public string Color { get; set; }
    }
}
