using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AprioritWebCalendar.Web.SignalR.Notifications
{
    public class NotificationHubManager
    {
        // TODO: Get invitations by SignalR.

        private readonly IHubContext<NotificationHub> _hub;

        public NotificationHubManager(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task CalendarSharedAsync(int sharedUserId, string calendarName, string calendarOwner)
        {
            await _hub.Clients.Group(sharedUserId.ToString()).SendAsync("calendarShared", new
            {
                calendarName,
                calendarOwner
            });
        }

        public async Task CalendarEditedAsync(int calendarOwnerId, string editedByUser, string calendarName, string newCalendarName)
        {
            await _hub.Clients.Group(calendarOwnerId.ToString()).SendAsync("calendarEdited", new
            {
                editedByUser,
                calendarName,
                newCalendarName
            });
        }

        public async Task EventInCalendarCreatedAsync(int calendarOwnerId, string createdByUser, string eventName, string calendarName)
        {
            await _hub.Clients.Group(calendarOwnerId.ToString()).SendAsync("eventInCalendarCreated", new
            {
                createdByUser,
                eventName,
                calendarName
            });
        }

        public async Task EventEditedAsync(int eventOwnerId, string editedByUser, string eventName, string newEventName)
        {
            await _hub.Clients.Group(eventOwnerId.ToString()).SendAsync("eventEdited", new
            {
                editedByUser,
                eventName,
                newEventName
            });
        }

        public async Task UserInvitedAsync(int userId, string eventName, string invitatorName)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("invited", new
            {
                eventName,
                invitatorName
            });
        }

        public async Task InvitationAcceptedAsync(int invitatorId, string eventName, string userName)
        {
            await _hub.Clients.Group(invitatorId.ToString()).SendAsync("invitationAccepted", new
            {
                eventName,
                userName
            });
        }

        public async Task InvitationRejectedAsync(int invitatorId, string eventName, string userName)
        {
            await _hub.Clients.Group(invitatorId.ToString()).SendAsync("invitationRejected", new
            {
                eventName,
                userName
            });
        }

        public async Task InvitationDeletedAsync(int userId, string eventName, string invitatorName)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("invitationDeleted", new
            {
                eventName,
                invitatorName
            });
        }

        public async Task RemovedFromCalendarAsync(int userId, string calendarName, string calendarOwner)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("removedFromCalendar", new
            {
                calendarName,
                calendarOwner
            });
        }

        public async Task RemovedFromEventAsync(int userId, string eventName, string eventOwner)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("removedFromEvent", new
            {
                eventName,
                eventOwner
            });
        }

        public async Task CalendarReadOnlyChangedAsync(int userId, string calendarName, string calendarOwner, bool isReadOnly)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("calendarReadOnlyChanged", new
            {
                calendarName,
                calendarOwner,
                isReadOnly
            });
        }

        public async Task EventReadOnlyChangedAsync(int userId, string eventName, string eventOwner, bool isReadOnly)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("eventReadOnlyChanged", new
            {
                eventName,
                eventOwner,
                isReadOnly
            });
        }
    }
}
