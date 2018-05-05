using Microsoft.AspNetCore.Authorization;

namespace AprioritWebCalendar.Web.SignalR.Notifications
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : BaseHub
    {     
    }
}
