using System;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class Period
    {
        public PeriodType Type { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int? Cycle { get; set; }
        public int WholeDaysCount { get; set; }
    }
}
