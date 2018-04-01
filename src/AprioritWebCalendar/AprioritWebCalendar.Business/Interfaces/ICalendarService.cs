using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.ViewModel.Calendar;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICalendarService
    {
        Task<IEnumerable<Calendar>> GetCalendarsAsync(int userId, bool onlyOwn = false);
        Task<Calendar> GetCalendarByIdAsync(int calendarId);

        Task<Calendar> CreateCalendarAsync(CalendarShortModel createModel, int ownerId);
        Task<Calendar> UpdateCalendarAsync(int calendarId, CalendarShortModel updateModel);
        Task DeleteCalendarAsync(int calendarId);

        Task ShareCalendarAsync(int calendarId, int userId, bool isReadOnly = true);
        Task RemoveSharingAsync(int calendarId, int userId);

        Task SubscribeCalendarAsync(int calendarId, int userId);
        Task UnsunscribeCalendarAsync(int calendarId, int userId);

        Task<bool> IsOwnerAsync(int calendarId, int userId);
    }
}
