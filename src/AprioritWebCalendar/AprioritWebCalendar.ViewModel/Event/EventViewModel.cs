using System;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }
        public int? RemindBefore { get; set; }
        public bool IsPrivate { get; set; } = true;

        public LocationViewModel Location { get; set; }
        public PeriodViewModel Period { get; set; }
        public UserViewModel Owner { get; set; }

        public int? CalendarId { get; set; }
        public string Color { get; set; }
        public bool? IsReadOnly { get; set; }
    }
}
