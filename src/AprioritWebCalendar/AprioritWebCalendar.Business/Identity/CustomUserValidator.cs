using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Data.Models;

namespace AprioritWebCalendar.Business.Identity
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (await manager.FindByNameAsync(user.UserName) != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserName",
                    Description = $"UserName \"{user.UserName}\" is already taken."
                });
            }

            if (await manager.FindByEmailAsync(user.Email) != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Email",
                    Description = $"An account with Email \"{user.Email}\" already exists."
                });
            }

            return IdentityResult.Success;
        }
    }
}
