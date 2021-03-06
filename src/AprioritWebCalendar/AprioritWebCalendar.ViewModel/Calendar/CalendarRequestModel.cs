﻿using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.ViewModel.Calendar
{
    public class CalendarRequestModel
    {
        [StringLength(32, MinimumLength = 3), Required]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [RegularExpression("^((0x){0,1}|#{0,1})([0-9A-F]{8}|[0-9A-F]{6})$")]
        [StringLength(7), Required]
        public string Color { get; set; }
    }
}
