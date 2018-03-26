using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetUsers")]
    public partial class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            AspNetUserClaims = new HashSet<ApplicationUserClaim>();
            AspNetUserLogins = new HashSet<ApplicationUserLogin>();
            AspNetUserRoles = new HashSet<ApplicationUserRole>();
            AspNetUserTokens = new HashSet<ApplicationUserToken>();
        }

        public ICollection<ApplicationUserClaim> AspNetUserClaims { get; set; }
        public ICollection<ApplicationUserLogin> AspNetUserLogins { get; set; }
        public ICollection<ApplicationUserRole> AspNetUserRoles { get; set; }
        public ICollection<ApplicationUserToken> AspNetUserTokens { get; set; }
    }
}
