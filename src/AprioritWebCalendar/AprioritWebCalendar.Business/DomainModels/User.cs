using System;
using System.Collections.Generic;
using AprioritWebCalendar.Infrastructure.DataTypes;

namespace AprioritWebCalendar.Business.DomainModels
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public TimeZoneInfoIana TimeZone { get; set; }
    }
}
