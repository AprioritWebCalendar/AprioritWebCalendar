using System;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventSearchResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }

        public PeriodViewModel Period { get; set; }
    }
}
