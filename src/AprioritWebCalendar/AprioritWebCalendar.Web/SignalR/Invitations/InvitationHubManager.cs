using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AprioritWebCalendar.ViewModel.Event;

namespace AprioritWebCalendar.Web.SignalR.Invitations
{
    public class InvitationHubManager : HubManager<InvitationHub>
    {
        public InvitationHubManager(IHubContext<InvitationHub> hub) : base(hub)
        {
        }

        public async Task UserInvitedAsync(int userId, InvitationViewModel invitation)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("invited", new { invitation });
        }

        public async Task InvitationDeletedAsync(int userId, int eventId, string eventName, string invitatorName)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("invitationDeleted", new
            {
                eventName,
                eventId,
                invitatorName
            });
        }
    }
}
