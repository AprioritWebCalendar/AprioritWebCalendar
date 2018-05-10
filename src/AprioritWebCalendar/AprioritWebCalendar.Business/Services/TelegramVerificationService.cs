using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Business.Services
{
    public class TelegramVerificationService : ITelegramVerificationService
    {
        public Task<bool> TryVerifyAsync(int userId, string code)
        {
            throw new NotImplementedException();
        }
    }
}
