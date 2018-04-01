using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AprioritWebCalendar.Data.Models
{
    public class UserCalendar
    {
        public int UserId { get; set; }
        public int CalendarId { get; set; }

        public bool IsReadOnly { get; set; } = true;
        public bool IsSubscribed { get; set; }

        public ApplicationUser User { get; set; }
        public Calendar Calendar { get; set; }
    }
}
