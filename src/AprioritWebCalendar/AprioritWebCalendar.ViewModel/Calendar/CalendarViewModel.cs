using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Calendar
{
    public class CalendarViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public UserViewModel Owner { get; set; }

        public bool? IsDefault { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? IsSubscribed { get; set; }
    }
}
