using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventMoveRequest
    {
        [Required]
        public int? OldCalendar { get; set; }

        [Required]
        public int? NewCalendar { get; set; }
    }
}
