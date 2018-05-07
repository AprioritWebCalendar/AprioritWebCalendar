using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using AprioritWebCalendar.ViewModel.Calendar;

namespace AprioritWebCalendar.Web.SignalR.Calendar
{
    public class CalendarHubManager : HubManager<CalendarHub>
    {
        public CalendarHubManager(IHubContext<CalendarHub> hub) : base(hub)
        {
        }

        public async Task CalendarSharedAsync(int sharedUserId, CalendarViewModel calendar)
        {
            await _hub.Clients.Group(sharedUserId.ToString()).SendAsync("calendarShared", new { calendar });
        }

        public async Task CalendarDeletedAsync(IEnumerable<int> usersId, int calendarId, string calendarName, string ownerName)
        {
            await _hub.Clients.Groups(usersId.Select(i => i.ToString()).ToList()).SendAsync("calendarDeleted", new
            {
                Calendar = new
                {
                    Id = calendarId,
                    Name = calendarName,
                    Owner = ownerName
                }
            });
        }

        public async Task RemovedFromCalendarAsync(int userId, int calendarId, string calendarName, string ownerName)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("removedFromCalendar", new
            {
                Calendar = new
                {
                    Id = calendarId,
                    Name = calendarName,
                    Owner = ownerName
                }
            });
        }

        public async Task CalendarEditedAsync(int calendarOwnerId, string editedByUser, CalendarViewModel calendar, string oldCalendarName)
        {
            await _hub.Clients.Group(calendarOwnerId.ToString()).SendAsync("calendarEdited", new
            {
                editedByUser,
                calendar,
                oldCalendarName
            });
        }

        public async Task CalendarReadOnlyChangedAsync(int userId, int calendarId, string calendarName, string calendarOwner, bool isReadOnly)
        {
            await _hub.Clients.Group(userId.ToString()).SendAsync("calendarReadOnlyChanged", new
            {
                Calendar = new
                {
                    Id = calendarId,
                    Name = calendarName,
                    Owner = calendarOwner
                },

                IsReadOnly = isReadOnly
            });
        }
    }
}
