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

        /// <summary>
        /// This method is to find users to share calendar or invite on event.
        /// It finds users which have "UserName" or "Email" that contains "emailOrUserName".
        /// The method mustn't return user which sends request.
        /// </summary>
        /// <param name="emailOrUserName">Email or UserName</param>
        /// <param name="currentUserId">ID of current user.</param>
        /// <returns></returns>
        Task<IEnumerable<User>> FindUsersAsync(string emailOrUserName, int currentUserId);

        Task AssignTelegramIdAsync(int userId, int telegramId);
        Task ResetTelegramIdAsync(int userId);
        Task EnableTelegramNotificationsAsync(int userId);
        Task DisableTelegramNotificationsAsync(int userId);
    }
}
