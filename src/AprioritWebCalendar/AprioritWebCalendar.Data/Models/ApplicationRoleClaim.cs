using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetRoleClaims")]
    public partial class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public ApplicationRole Role { get; set; }
    }
}
