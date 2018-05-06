using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AprioritWebCalendar.Web.Extensions;

namespace AprioritWebCalendar.Web.SignalR
{
    public abstract class BaseHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Groups.AddAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await Groups.RemoveAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
        }
    }
}
