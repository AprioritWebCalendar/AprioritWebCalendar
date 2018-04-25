namespace AprioritWebCalendar.Business.DomainModels
{
    /// <summary>
    /// This class is for the method which returns a list of users,
    /// which are invited on event.
    /// So it's necessary to have a property, that shows user accepted invitation, or no.
    /// </summary>
    public class UserInvitation
    {
        public User User { get; set; }

        /// <summary>
        /// TRUE - user accepted the event.
        /// FALSE - user HAS NOT accepted...
        /// </summary>
        public bool IsAccepted { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
