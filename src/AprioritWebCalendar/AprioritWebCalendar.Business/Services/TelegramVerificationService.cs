using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.Identity;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.Exceptions;

namespace AprioritWebCalendar.Business.Services
{
    public class TelegramVerificationService : ITelegramVerificationService
    {
        private readonly IRepository<TelegramCode> _codesRepository;
        private readonly IRandomDataProvider _randomDataProvider;
        private readonly CustomUserManager _userManager;

        public TelegramVerificationService(IRepository<TelegramCode> codesRepository, IRandomDataProvider randomDataProvider, CustomUserManager userManager)
        {
            _codesRepository = codesRepository;
            _randomDataProvider = randomDataProvider;
            _userManager = userManager;
        }

        public async Task<int> TryVerifyAsync(int userId, string code)
        {
            var telegramIdCode = (await _codesRepository.FindAllAsync(c => c.Code.Equals(code)))
                .FirstOrDefault();

            if (telegramIdCode == null)
                throw new NotFoundException();

            await _codesRepository.RemoveAsync(telegramIdCode);

            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AssignTelegramIdAsync(user, telegramIdCode.TelegramId);

            return telegramIdCode.TelegramId;
        }

        public async Task<string> GetVerificationCodeAsync(int telegramId)
        {
            string code = null;

            do
            {
                code = _randomDataProvider.GetRandomBase64String(32);
            }
            while (await _codesRepository.AnyAsync(c => c.Code.Equals(code)));

            await _codesRepository.CreateAsync(new TelegramCode
            {
                TelegramId = telegramId,
                Code = code
            });
            await _codesRepository.SaveAsync();

            return code;
        }
    }
}
