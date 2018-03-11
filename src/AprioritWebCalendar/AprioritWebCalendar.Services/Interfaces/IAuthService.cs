using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.Services.DTO;

namespace AprioritWebCalendar.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDTO);
        Task<UserDTO> FindUserByCredentialsAsync(LoginDTO loginDTO);
    }
}
