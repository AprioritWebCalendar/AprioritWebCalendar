using System.ComponentModel.DataAnnotations;

namespace AprioritWebCalendar.ViewModel.Account
{
    public class LoginRequestModel
    {
        [Required]
        public string EmailOrUserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
