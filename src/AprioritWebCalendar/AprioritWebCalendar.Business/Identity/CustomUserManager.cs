using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AprioritWebCalendar.Infrastructure.Exceptions;

namespace AprioritWebCalendar.Business.Identity
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        public CustomUserManager(
            IUserStore<ApplicationUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<ApplicationUser>> logger,
            
            IRepository<ApplicationUser> userRepository) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _userRepository = userRepository;
        }

        protected readonly IRepository<ApplicationUser> _userRepository;

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException();

            return user;
        }

        public async Task<ApplicationUser> FindByTelegramIdAsync(string telegramId)
        {
            return (await _userRepository.FindAllAsync(u => u.TelegramId.Equals(telegramId)))
                .FirstOrDefault();
        }

        public async Task AssignTelegramIdAsync(ApplicationUser user, string telegramId)
        {
            user.TelegramId = telegramId;
            user.IsTelegramNotificationEnabled = true;
            await UpdateAsync(user);
        }

        public async Task ResetTelegramIdAsync(ApplicationUser user)
        {
            user.TelegramId = null;
            user.IsTelegramNotificationEnabled = null;
            await UpdateAsync(user);
        }

        public async Task SetTelegramNotificationsEnableAsync(ApplicationUser user, bool status)
        {
            user.IsTelegramNotificationEnabled = status;
            await UpdateAsync(user);
        }
    }
}
