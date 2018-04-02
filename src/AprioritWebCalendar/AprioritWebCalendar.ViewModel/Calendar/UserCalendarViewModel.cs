using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Calendar
{
    public class UserCalendarViewModel
    {
        public UserViewModel User { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
