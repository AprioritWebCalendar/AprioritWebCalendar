using Microsoft.AspNetCore.SignalR;

namespace AprioritWebCalendar.Web.SignalR
{
    public abstract class HubManager<THub> where THub : Hub
    {
        protected IHubContext<THub> _hub;

        public HubManager(IHubContext<THub> hub)
        {
            _hub = hub;
        }
    }
}
