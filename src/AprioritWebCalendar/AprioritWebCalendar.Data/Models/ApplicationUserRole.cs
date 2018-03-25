using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetUserRoles")]
    public partial class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationRole Role { get; set; }
        public ApplicationUser User { get; set; }
    }
}
