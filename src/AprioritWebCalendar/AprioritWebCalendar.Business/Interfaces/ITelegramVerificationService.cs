using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ITelegramVerificationService
    {
        Task<int> TryVerifyAsync(int userId, string code);
        Task<string> GetVerificationCodeAsync(int telegramId);
        Task<bool> CodeExistsAsync(int telegramId);
        Task RemoveCodesAsync(int telegramId);
    }
}
