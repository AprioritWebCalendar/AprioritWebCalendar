using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICaptchaService
    {
        Task<bool> TryVerifyCaptchaAsync(string token);
    }
}
