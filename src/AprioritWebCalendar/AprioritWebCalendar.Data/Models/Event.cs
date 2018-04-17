using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AprioritWebCalendar.Data.Models
{
    public class Event
    {
        public Event()
        {
            Calendars = new HashSet<EventCalendar>();
            Invitations = new HashSet<Invitation>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
        public string LocationDescription { get; set; }

        [ForeignKey(nameof(Owner))]
        public int OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public bool IsAllDay { get; set; }
        
        public int? RemindBefore { get; set; }
        public bool IsPrivate { get; set; } = true;

        public Period Period { get; set; }

        public ICollection<EventCalendar> Calendars { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}
