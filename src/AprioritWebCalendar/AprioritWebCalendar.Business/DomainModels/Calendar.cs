using System;
using System.Collections.Generic;
using System.Text;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public User Owner { get; set; }

        public bool? IsDefault { get; set; }

        public IEnumerable<UserCalendar> SharedUsers { get; set; } = new List<UserCalendar>();
        public IEnumerable<EventCalendar> Events { get; set; } = new List<EventCalendar>();
    }
}
