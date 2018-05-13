using System;

namespace AprioritWebCalendar.Data.Models
{
    public class TelegramCode
    {
        public int TelegramId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
