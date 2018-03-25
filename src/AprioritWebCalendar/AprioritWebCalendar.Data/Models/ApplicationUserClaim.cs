using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetUserClaims")]
    public partial class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public ApplicationUser User { get; set; }
    }
}
