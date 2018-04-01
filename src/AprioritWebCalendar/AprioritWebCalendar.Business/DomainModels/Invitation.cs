using System;
using System.Collections.Generic;
using System.Text;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class Invitation
    {
        public Event Event { get; set; }
        public User User { get; set; }
        public User Invitator { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
