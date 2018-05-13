using Microsoft.AspNetCore.Authorization;

namespace AprioritWebCalendar.Web.SignalR.Telegram
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TelegramHub : BaseHub
    {
    }
}
