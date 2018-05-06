using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string message);
    }
}
