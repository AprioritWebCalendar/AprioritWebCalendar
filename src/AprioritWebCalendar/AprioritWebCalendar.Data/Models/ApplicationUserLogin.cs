using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    [Table("AspNetUserLogins")]
    public partial class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public ApplicationUser User { get; set; }
    }
}
