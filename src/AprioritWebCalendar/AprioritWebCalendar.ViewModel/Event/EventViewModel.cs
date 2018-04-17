using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [RegularExpression("^([0-9]{2}/[0-9]{2}/[0-9]{4})$")]
        public string StartDate { get; set; }
        [RegularExpression("^([0-9]{2}/[0-9]{2}/[0-9]{4})$")]
        public string EndDate { get; set; }
        [RegularExpression("^([0-9]{2}:[0-9]{2}:[0-9]{2})$")]
        public string StartTime { get; set; }
        [RegularExpression("^([0-9]{2}:[0-9]{2}:[0-9]{2})$")]
        public string EndTime { get; set; }
        public bool isRecurring { get; set; }
        public int CalendarId { get; set; }

    }
}
