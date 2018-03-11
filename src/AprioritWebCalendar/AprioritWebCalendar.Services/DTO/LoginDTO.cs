using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.Services.DTO
{
    public class LoginDTO
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
