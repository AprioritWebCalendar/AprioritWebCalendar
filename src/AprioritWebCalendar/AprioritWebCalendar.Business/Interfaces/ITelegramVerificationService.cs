using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ITelegramVerificationService
    {
        Task<bool> TryVerifyAsync(int userId, string code);
        Task<string> GetVerificationCodeAsync(int telegramId);
    }
}
