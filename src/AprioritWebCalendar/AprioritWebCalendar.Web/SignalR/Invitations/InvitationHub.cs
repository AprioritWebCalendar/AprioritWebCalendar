using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.ViewModel.Event;
using AprioritWebCalendar.Web.SignalR.Notifications;

namespace AprioritWebCalendar.Web.SignalR.Invitations
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InvitationHub : BaseHub
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        private readonly NotificationHubManager _notificationManager;

        public InvitationHub(IEventService eventService, IMapper mapper, NotificationHubManager notificationHubManager)
        {
            _eventService = eventService;
            _mapper = mapper;
            _notificationManager = notificationHubManager;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var userId = Context.User.GetUserId();
            var invitations = await _eventService.GetIncomingInvitationsAsync(userId);

            if (invitations.Any())
            {
                var viewModels = _mapper.Map<IEnumerable<InvitationViewModel>>(invitations);
                await Clients.Group(userId.ToString()).SendAsync("incomingInvitations", viewModels);
            }
        }
    }
}
