using System;
using System.Collections.Generic;
using System.Text;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class PeriodViewModel
    {
        public PeriodType Type { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int? Cycle { get; set; }
        public int WholeDaysCount { get; set; }
    }
}
