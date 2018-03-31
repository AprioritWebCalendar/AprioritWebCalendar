using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    public partial class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            UserRoles = new HashSet<IdentityUserRole<int>>();
        }
        
        public ICollection<IdentityUserRole<int>> UserRoles { get; set; }
    }
}
