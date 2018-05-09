using System.ComponentModel.DataAnnotations;
using AprioritWebCalendar.Infrastructure.Validation;

namespace AprioritWebCalendar.ViewModel.Account
{
    public class RegisterRequestModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(16, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required, StringLength(64, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, IanaTimeZone]
        public string TimeZone { get; set; }
    }
}
