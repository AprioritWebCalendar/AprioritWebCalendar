using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityResult> CreateUserAsync(RegisterRequestModel registerModel);
        Task<User> GetUserAsync(int id);
    }
}
