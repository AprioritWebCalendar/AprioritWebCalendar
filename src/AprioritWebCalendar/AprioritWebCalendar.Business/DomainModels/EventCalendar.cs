using System;
using System.Collections.Generic;
using System.Text;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class EventCalendar
    {
        public Event Event { get; set; }
        public Calendar Calendar { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
