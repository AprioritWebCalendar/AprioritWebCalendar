using Microsoft.AspNetCore.Authorization;

namespace AprioritWebCalendar.Web.SignalR.Calendar
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CalendarHub : BaseHub
    {
    }
}
