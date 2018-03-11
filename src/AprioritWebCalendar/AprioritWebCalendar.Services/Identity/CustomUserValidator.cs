using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Data.Entities;

namespace AprioritWebCalendar.Services.Identity
{
    public class CustomUserValidator : IUserValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
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
