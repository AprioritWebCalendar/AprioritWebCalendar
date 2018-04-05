using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AprioritWebCalendar.Data.Models
{
    public class Calendar
    {
        public Calendar()
        {
            EventCalendars = new HashSet<EventCalendar>();
            SharedUsers = new HashSet<UserCalendar>();
        }

        public int Id { get; set; }

        [StringLength(32), Required]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }

        [Required, StringLength(7)]
        public string Color { get; set; }
        public bool IsDefault { get; set; }
        
        [ForeignKey(nameof(Owner))]
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public ICollection<EventCalendar> EventCalendars { get; set; }
        public ICollection<UserCalendar> SharedUsers { get; set; }
    }
}
