using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.ViewModel.Calendar
{
    public class CalendarShareRequest
    {
        [Required]
        public int? UserId { get; set; }
        public bool IsReadOnly { get; set; } = true;
    }
}
