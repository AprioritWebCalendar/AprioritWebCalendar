using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Data.Models;

namespace AprioritWebCalendar.Business.Identity
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            // TODO: Validate nickname for incorrect symbols.

            if (manager.Users.Any(u => u.UserName.Equals(user.UserName) && u.Id != user.Id))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UserName",
                    Description = $"UserName \"{user.UserName}\" is already taken."
                }));
            }

            if (manager.Users.Any(u => u.Email.Equals(user.Email) && u.Id != user.Id))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "Email",
                    Description = $"An account with Email \"{user.Email}\" already exists."
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
