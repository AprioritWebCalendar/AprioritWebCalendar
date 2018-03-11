using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.Services.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
