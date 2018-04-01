using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    public partial class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole()
        {
            UserRoles = new HashSet<IdentityUserRole<int>>();
        }
        
        public ICollection<IdentityUserRole<int>> UserRoles { get; set; }
    }
}
