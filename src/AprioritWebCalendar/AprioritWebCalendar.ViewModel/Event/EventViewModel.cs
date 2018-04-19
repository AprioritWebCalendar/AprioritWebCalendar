using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [StringLength(32, MinimumLength = 3), Required]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }
        public int? RemindBefore { get; set; }

        public LocationViewModel Location { get; set; }
        public PeriodViewModel Period { get; set; }
        public UserViewModel Owner { get; set; }

        public int CalendarId { get; set; }
        public string Color { get; set; }
    }
}
