using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.DataTypes;
using AprioritWebCalendar.Infrastructure.Exceptions;

namespace AprioritWebCalendar.Business.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IRepository<ApplicationUser> _userRepository;

        public SettingsService(IRepository<ApplicationUser> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<TimeZoneInfoIana> GetTimeZoneAsync(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException();

            return new TimeZoneInfoIana(user.TimeZone);
        }

        public async Task SetTimeZoneAsync(int userId, TimeZoneInfoIana timeZone)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException();

            user.TimeZone = timeZone.ToString();
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveAsync();
        }
    }
}
