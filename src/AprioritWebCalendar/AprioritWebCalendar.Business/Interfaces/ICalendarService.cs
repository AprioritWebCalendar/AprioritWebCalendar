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
        /// <summary>
        /// Gets list of calendars that user is assigned with.
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="onlyOwn">If true, the method returns only user's own calendars.</param>
        /// <returns>Enumeration of calendars (domain models).</returns>
        Task<IEnumerable<Calendar>> GetCalendarsAsync(int userId, bool onlyOwn = false);

        /// <summary>
        /// Gets a calendar by Id.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <returns>Calendar domain model.</returns>
        Task<Calendar> GetCalendarByIdAsync(int calendarId);

        /// <summary>
        /// Creates a new calendar.
        /// </summary>
        /// <param name="createModel">View (binding model).</param>
        /// <param name="ownerId">ID of the users that's owner.</param>
        Task<Calendar> CreateCalendarAsync(CalendarShortModel createModel, int ownerId);

        /// <summary>
        /// Updates an existing calendar.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="updateModel">View (binding model).</param>
        Task<Calendar> UpdateCalendarAsync(int calendarId, CalendarShortModel updateModel);

        /// <summary>
        /// Deletes an existing calendar.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        Task DeleteCalendarAsync(int calendarId);

        /// <summary>
        /// Shares a calendar with user.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        /// <param name="isReadOnly">Access mode for user share with.</param>
        Task ShareCalendarAsync(int calendarId, int userId, bool isReadOnly = true);

        /// <summary>
        /// Removes calendar's sharing from user.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        Task RemoveSharingAsync(int calendarId, int userId);

        /// <summary>
        /// Subscribes user for a calendar.
        /// If user's subscribed, it gets notifications about events from the calendar.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        Task SubscribeCalendarAsync(int calendarId, int userId);

        /// <summary>
        /// Unsubscribes user for a calendar.
        /// If user's subscribed, it gets notifications about events from the calendar.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        Task UnsunscribeCalendarAsync(int calendarId, int userId);

        /// <summary>
        /// Gets list of users that calendar shared with.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <returns>Enumeration of users.</returns>
        Task<IEnumerable<UserCalendar>> GetUsersSharedWithAsync(int calendarId);

        /// <summary>
        /// Checks user is owner of calendar.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        /// <returns>true - user is owner of the calendar, false - no.</returns>
        Task<bool> IsOwnerAsync(int calendarId, int userId);

        /// <summary>
        /// Checks user is owner or the calendar shared with him.
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        /// <returns></returns>
        Task<bool> IsOwnerOrSharedWithAsync(int calendarId, int userId);

        /// <summary>
        /// Checks user permissions to edit a calendar.
        /// It can do owner or user that a calendar shared with him and have !IsReadOnly
        /// </summary>
        /// <param name="calendarId">ID of the calendar.</param>
        /// <param name="userId">ID of the user.</param>
        /// <returns>true - can edit, false - no</returns>
        Task<bool> CanEditAsync(int calendarId, int userId);
    }
}
