using System.Collections.Generic;

namespace AprioritWebCalendar.ViewModel.Account
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public string TimeZone { get; set; }
    }
}
