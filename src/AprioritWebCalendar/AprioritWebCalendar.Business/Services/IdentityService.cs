using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.Business.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterRequestModel registerModel)
        {
            var user = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName
            };

            return await _userManager.CreateAsync(user, registerModel.Password);
        }
    }
}
