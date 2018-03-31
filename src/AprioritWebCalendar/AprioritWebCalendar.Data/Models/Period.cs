using System;
using System.ComponentModel.DataAnnotations;
using AprioritWebCalendar.Infrastructure.Enums;

namespace AprioritWebCalendar.Data.Models
{
    public class Period
    {
        [Key]
        public int EventId { get; set; }
        public Event Event { get; set; }

        public PeriodType Type { get; set; }
        
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int WholeDaysCount { get; set; }

        /// <summary>
        /// Only if "Type" is "Custom".
        /// </summary>
        public int? CycleDays { get; set; }

        // TODO: WeekDays (flags).
    }
}
