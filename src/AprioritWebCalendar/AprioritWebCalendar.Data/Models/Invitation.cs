using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AprioritWebCalendar.Data.Models
{
    public class Invitation
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int EventId { get; set; }

        [ForeignKey(nameof(Invitator))]
        [Key]
        public int InvitatorId { get; set; }

        public bool IsReadOnly { get; set; }

        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
        public ApplicationUser Invitator { get; set; }
    }
}
