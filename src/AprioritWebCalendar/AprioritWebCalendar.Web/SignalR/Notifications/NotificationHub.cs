using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using AprioritWebCalendar.Web.Extensions;

namespace AprioritWebCalendar.Web.SignalR.Notifications
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await Groups.AddAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            await base.OnDisconnectedAsync(exception);
        }
    }
}
