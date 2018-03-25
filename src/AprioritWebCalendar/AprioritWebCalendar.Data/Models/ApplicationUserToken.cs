using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetUserTokens")]
    public partial class ApplicationUserToken : IdentityUserToken<int>
    {
        public ApplicationUser User { get; set; }
    }
}
