using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Gets events.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="calendarsIds">IDs of calendars to show events from.</param>
        /// <returns>Enumeration of events.</returns>
        Task<IEnumerable<Event>> GetEventsAsync(int userId, DateTime startDate, DateTime endDate, params int[] calendarsIds);

        /// <summary>
        /// Gets event by ID without related entities.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <returns>Event</returns>
        Task<Event> GetEventByIdAsync(int eventId);

        /// <summary>
        /// Gets event by ID with related entities.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="includeProperties">Related entities.</param>
        /// <returns>Event</returns>
        Task<Event> GetEventByIdAsync(int eventId, params string[] includeProperties);

        /// <summary>
        /// Gets event by ID with relates entities.
        /// Also gets IsReadOnly, CalendarId and Color for user.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">UserID</param>
        /// <returns>Event</returns>
        Task<Event> GetEventByIdAsync(int eventId, int userId);

        /// <summary>
        /// Gets incoming invitations.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Enumeration of invitations.</returns>
        Task<IEnumerable<Invitation>> GetIncomingInvitationsAsync(int userId);

        /// <summary>
        /// Gets outcoming invitations.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Enumeration of invitations.</returns>
        Task<IEnumerable<Invitation>> GetOutcomingInvitationsAsync(int userId);

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventDomain">Event domain model</param>
        /// <param name="ownerId">Owner ID</param>
        /// <returns>Id of created event.</returns>
        Task<int> CreateEventAsync(Event eventDomain, int ownerId);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="eventDomain">Event domain model</param>
        Task UpdateEventAsync(Event eventDomain);

        /// <summary>
        /// Deletes an existing event.
        /// </summary>
        /// <param name="eventId"></param>
        Task DeleteEventAsync(int eventId);

        /// <summary>
        /// Moves an event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="oldCalendarId">Old calendar ID</param>
        /// <param name="calendarId">New calendar ID</param>
        Task MoveEventAsync(int eventId, int oldCalendarId, int calendarId);

        /// <summary>
        /// Gets a list of users which are invited on event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <returns>Enumeration of users.</returns>
        Task<IEnumerable<UserInvitation>> GetInvitedUsersAsync(int eventId);

        /// <summary>
        /// Invites user to event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User (to invite) ID</param>
        /// <param name="invitatorId">Invitator ID</param>
        /// <param name="isReadOnly">Permissions for invited user</param>
        Task InviteUserAsync(int eventId, int userId, int invitatorId, bool isReadOnly);

        /// <summary>
        /// Accepts invitation to event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        Task AcceptInvatationAsync(int eventId, int userId);

        /// <summary>
        /// Rejects (cancel) invitation to event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        Task RejectInvitationAsync(int eventId, int userId);

        /// <summary>
        /// Deletes invited user.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        Task DeleteIntvitedUserAsync(int eventId, int userId);

        /// <summary>
        /// Updates permissions for invitation.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="isReadOnly">Permissions</param>
        Task UpdateInvitationReadOnlyAsync(int eventId, int userId, bool isReadOnly);

        /// <summary>
        /// Updates permissions for invited user.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        /// <param name="isReadOnly">Permissions</param>
        Task UpdateEventReadOnlyStateAsync(int eventId, int userId, bool isReadOnly);

        /// <summary>
        /// Checks event is private.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>Private or no.</returns>
        Task<bool> IsPrivateAsync(int eventId);

        /// <summary>
        /// Checks user is owner of event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        Task<bool> IsOwnerAsync(int eventId, int userId);

        /// <summary>
        /// Checks user is owner or invited on event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        Task<bool> IsOwnerOrInvitedAsync(int eventId, int userId);

        /// <summary>
        /// Checks can user edit event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        Task<bool> CanEditAsync(int eventId, int userId);
    }
}
