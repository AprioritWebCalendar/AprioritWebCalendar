using System;
using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.Data.Models
{
    public class TelegramCode
    {
        [Key]
        public int TelegramId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
