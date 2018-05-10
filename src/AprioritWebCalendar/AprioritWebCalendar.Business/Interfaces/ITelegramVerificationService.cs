using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ITelegramVerificationService
    {
        Task<bool> TryVerifyAsync(int userId, string code);
    }
}
