namespace AprioritWebCalendar.Business.DomainModels
{
    public class UserCalendar
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int CalendarId { get; set; }
        public Calendar Calendar { get; set; }

        public bool IsReadOnly { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
