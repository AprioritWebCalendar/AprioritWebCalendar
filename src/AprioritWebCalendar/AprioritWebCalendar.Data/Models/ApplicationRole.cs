using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetRoles")]
    public partial class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
            AspNetRoleClaims = new HashSet<ApplicationRoleClaim>();
            AspNetUserRoles = new HashSet<ApplicationUserRole>();
        }

        public ICollection<ApplicationRoleClaim> AspNetRoleClaims { get; set; }
        public ICollection<ApplicationUserRole> AspNetUserRoles { get; set; }
    }
}
