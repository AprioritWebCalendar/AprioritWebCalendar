using System;
using System.Collections.Generic;
using System.Text;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class UserCalendar
    {
        public User User { get; set; }
        public Calendar Calendar { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
