using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace AprioritWebCalendar.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string TelegramId { get; set; }
    }
}
