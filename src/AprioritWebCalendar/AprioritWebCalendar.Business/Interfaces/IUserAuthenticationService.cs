using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Infrastructure.Options;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<User> FindUserByCredentialsAsync(LoginRequestModel loginModel);
        ClaimsIdentity CreateClaims(User user);
        string CreateJwtToken(ClaimsIdentity claims, JwtOptions jwtOptions);
    }
}
