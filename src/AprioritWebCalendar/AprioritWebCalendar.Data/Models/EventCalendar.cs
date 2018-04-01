using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AprioritWebCalendar.Data.Models
{
    public class EventCalendar
    {
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        [ForeignKey(nameof(Calendar))]
        public int CalendarId { get; set; }

        public bool IsReadOnly { get; set; } = true;

        public Event Event { get; set; }
        public Calendar Calendar { get; set; }
    }
}
