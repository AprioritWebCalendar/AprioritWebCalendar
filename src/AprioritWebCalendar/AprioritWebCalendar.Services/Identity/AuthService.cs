using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Data.Entities;
using AprioritWebCalendar.Services.DTO;
using AprioritWebCalendar.Services.Interfaces;

namespace AprioritWebCalendar.Services.Identity
{
    public class AuthService : IAuthService
    {
        private UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDTO)
        {
            var user = new User
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName
            };

            return await _userManager.CreateAsync(user, registerDTO.Password);
        }

        public async Task<UserDTO> FindUserByCredentialsAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.UserNameOrEmail)
                ?? await _userManager.FindByNameAsync(loginDTO.UserNameOrEmail);

            if (user == null)
                return null;

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                return null;

            var dto = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };

            dto.Roles = await _userManager.GetRolesAsync(user);
            return dto;
        }
    }
}
