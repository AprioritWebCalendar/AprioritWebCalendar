using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.Data.Models
{
    public class TelegramCode
    {
        [Key]
        public string TelegramId { get; set; }
        public string Code { get; set; }
    }
}
