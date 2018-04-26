using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class InvitationViewModel
    {
        public EventViewModel Event { get; set; }
        public UserViewModel Invitator { get; set; }
        public UserViewModel User { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
