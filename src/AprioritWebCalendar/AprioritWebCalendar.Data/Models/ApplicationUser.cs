using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    public partial class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            UserRoles = new HashSet<IdentityUserRole<int>>();

            OwnCalendars = new HashSet<Calendar>();
            OwnEvents = new HashSet<Event>();

            SharedCalendars = new HashSet<UserCalendar>();

            IncomingInvitations = new HashSet<Invitation>();
            OutcomingInvitations = new HashSet<Invitation>();
        }

        /// <summary>
        /// Time offset from UTC in minutes.
        /// </summary>
        public int TimeOffset { get; set; }
        
        public ICollection<IdentityUserRole<int>> UserRoles { get; set; }

        public ICollection<Event> OwnEvents { get; set; }
        
        public ICollection<Calendar> OwnCalendars { get; set; }
        public ICollection<UserCalendar> SharedCalendars { get; set; }

        [InverseProperty(nameof(Invitation.User))]
        public ICollection<Invitation> IncomingInvitations { get; set; }

        [InverseProperty(nameof(Invitation.Invitator))]
        public ICollection<Invitation> OutcomingInvitations { get; set; }
    }
}
