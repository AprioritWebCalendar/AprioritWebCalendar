using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AprioritWebCalendar.Web.SignalR.Telegram
{
    public class TelegramHubManager : HubManager<TelegramHub>
    {
        public TelegramHubManager(IHubContext<TelegramHub> hub) : base(hub)
        {
        }

        public async Task TelegramResetedAsync(int userId)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("telegramReseted");
        }

        public async Task TelegramNotificationsEnabledAsync(int userId)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("telegramNotificationsEnabled");
        }

        public async Task TelegramNotificationsDisabledAsync(int userId)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("telegramNotificationsDisabled");
        }
    }
}
