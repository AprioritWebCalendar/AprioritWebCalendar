﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.Exceptions;
using AprioritWebCalendar.Infrastructure.Extensions;
using DomainEvent = AprioritWebCalendar.Business.DomainModels.Event;
using DomainEventCalendar = AprioritWebCalendar.Business.DomainModels.EventCalendar;
using DomainUser = AprioritWebCalendar.Business.DomainModels.User;

namespace AprioritWebCalendar.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventCalendar> _eventCalendarRepository;
        private readonly IRepository<UserCalendar> _userCalendarRepository;
        private readonly IRepository<Invitation> _invitationRepository;
        private readonly IRepository<Calendar> _calendarRepository;

        private readonly ICalendarService _calendarService;

        private readonly IMapper _mapper;

        public EventService(
            IRepository<Event> eventRepository,
            IRepository<EventCalendar> eventCalendarRepository,
            IRepository<UserCalendar> userCalendarRepository,
            IRepository<Invitation> invitationRepository,
            IRepository<Calendar> calendarRepository,
            ICalendarService calendarService,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _eventCalendarRepository = eventCalendarRepository;
            _userCalendarRepository = userCalendarRepository;
            _invitationRepository = invitationRepository;
            _calendarRepository = calendarRepository;

            _calendarService = calendarService;

            _mapper = mapper;
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(int userId, DateTime startDate, DateTime endDate, params int[] calendarsIds)
        {
            // TODO: Needs optimization and refactoring.

            Expression<Func<Event, bool>> filter = e => ((e.Period == null && e.StartDate >= startDate && e.EndDate <= endDate)
                || (e.Period != null && e.Period.PeriodStart >= startDate && e.Period.PeriodEnd <= endDate))
                && e.Calendars.Select(c => c.CalendarId).Intersect(calendarsIds).Any();

            var dataEvents = (await _eventRepository.FindAllIncludingAsync(filter, e => e.Calendars, e => e.Owner, e => e.Period))
                .ToList();

            if (dataEvents?.Any() != true)
                return null;

            var dataEventCalendars = (await _eventCalendarRepository.FindAllIncludingAsync(e => calendarsIds.Contains(e.CalendarId), c => c.Calendar))
                .ToList();
            
            var dataUserCalendars = (await _userCalendarRepository.FindAllIncludingAsync(u => calendarsIds.Contains(u.CalendarId) && u.UserId == userId, u => u.Calendar))
                .ToList();

            var userOwnCalendars = (await _calendarRepository.FindAllAsync(c => c.OwnerId == userId))
                .Select(c => new UserCalendar
                {
                    UserId = userId,
                    CalendarId = c.Id,
                    Calendar = c,
                    IsReadOnly = false
                });

            dataUserCalendars.AddRange(userOwnCalendars);

            var matchedEvents = (from @event in dataEvents
                                join evCalendar in dataEventCalendars on @event.Id equals evCalendar.EventId
                                join usCalendar in dataUserCalendars on evCalendar.CalendarId equals usCalendar.CalendarId
                                select new
                                {
                                    EventId = @event.Id,
                                    CalendarId = usCalendar.CalendarId,
                                    Color = usCalendar.Calendar.Color,
                                    IsReadOnly = usCalendar.IsReadOnly
                                }).ToList();

            dataEvents.RemoveAll(e => e.IsPrivate && e.OwnerId != userId);

            var domainEvents = _mapper.Map<List<DomainEvent>>(dataEvents);

            foreach (var ev in domainEvents)
            {
                var match = matchedEvents.FirstOrDefault(e => e.EventId == ev.Id);

                ev.CalendarId = match.CalendarId;
                ev.Color = match.Color;
                ev.IsReadOnly = match.IsReadOnly;
            }

            return domainEvents;
        }

        public async Task<DomainEvent> GetEventByIdAsync(int eventId)
        {
            var dataEvent = await _GetEventAsync(eventId);
            return _mapper.Map<DomainEvent>(dataEvent);
        }

        public async Task<DomainEvent> GetEventByIdAsync(int eventId, params string[] includeProperties)
        {
            var dataEvent = (await _eventRepository.FindAllIncludingAsync(e => e.Id == eventId, includeProperties))
                .FirstOrDefault();

            if (dataEvent == null)
                throw new NotFoundException();

            return _mapper.Map<DomainEvent>(dataEvent);
        }

        public async Task<DomainEvent> GetEventByIdAsync(int eventId, int userId)
        {
            var eventCalendar = (await _eventCalendarRepository.FindAllIncludingAsync(e => e.EventId == eventId && e.Calendar.OwnerId == userId, 
                    e => e.Calendar, e => e.Event, e => e.Event.Owner, e => e.Event.Period))
                .FirstOrDefault();

            if (eventCalendar == null)
                throw new NotFoundException();

            var domainEvent = _mapper.Map<DomainEvent>(eventCalendar.Event);

            domainEvent.IsReadOnly = eventCalendar.IsReadOnly;
            domainEvent.CalendarId = eventCalendar.CalendarId;
            domainEvent.Color = eventCalendar.Calendar.Color;

            return domainEvent;
        }

        public async Task<int> CreateEventAsync(DomainEvent eventDomain, int ownerId)
        {
            var dataEvent = _mapper.Map<Event>(eventDomain);
            dataEvent.OwnerId = ownerId;

            dataEvent.Calendars.Add(new EventCalendar
            {
                CalendarId = eventDomain.CalendarId,
                IsReadOnly = false
            });

            dataEvent = await _eventRepository.CreateAsync(dataEvent);
            await _eventRepository.SaveAsync();

            return dataEvent.Id;
        }

        public async Task UpdateEventAsync(DomainEvent eventDomain)
        {
            var dataEvent = await _GetEventAsync(eventDomain.Id);
            _mapper.MapToEntity(eventDomain, dataEvent);

            await _eventRepository.UpdateAsync(dataEvent);
            await _eventRepository.SaveAsync();
        }

        public async Task DeleteEventAsync(int eventId)
        {
            var ev = await _GetEventAsync(eventId, e => e.Owner, e => e.Calendars, e => e.Period, e => e.Invitations);

            await _eventRepository.RemoveAsync(ev);
            await _eventRepository.SaveAsync();
        }

        public async Task MoveEventAsync(int eventId, int oldCalendarId, int calendarId)
        {
            var eventCalendar = await _eventCalendarRepository.FindByKeysAsync(eventId, oldCalendarId);

            if (eventCalendar == null)
                throw new NotFoundException();

            await _eventCalendarRepository.RemoveAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();

            eventCalendar.CalendarId = calendarId;

            await _eventCalendarRepository.CreateAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();
        }

        public async Task<IEnumerable<DomainModels.UserInvitation>> GetInvitedUsersAsync(int eventId)
        {
            var ev = await _GetEventAsync(eventId);

            var userAcceptedEvent = (await _eventCalendarRepository.FindAllIncludingAsync(e => e.EventId == eventId, e => e.Calendar, e => e.Calendar.Owner))
                .Select(e => e.Calendar.Owner)
                .ToList();

            var invitedUsers = (await _invitationRepository.FindAllIncludingAsync(i => i.EventId == eventId, i => i.User))
                .Select(i => i.User)
                .ToList();

            // Removing event's owner from the list that matches events with calendars.
            userAcceptedEvent.RemoveAll(u => u.Id == ev.Id);

            // Mapping to "UserInvitation".
            var users = _mapper.Map<List<DomainModels.UserInvitation>>(userAcceptedEvent);

            // Setting "IsAccepted".
            foreach (var u in users)
            {
                u.IsAccepted = true;
            }

            // Adding users which haven't accepted.
            users.AddRange(_mapper.Map<List<DomainModels.UserInvitation>>(invitedUsers));

            return users;
        }

        public async Task InviteUserAsync(int eventId, int userId, int invitatorId, bool isReadOnly)
        {
            var eventCalendars = await _eventCalendarRepository.FindAllIncludingAsync(e => e.EventId == eventId && e.Calendar.OwnerId == userId,
                e => e.Calendar);

            // User is already invited.

            if (eventCalendars.Any())
                throw new InvalidOperationException();

            var invitation = new Invitation
            {
                EventId = eventId,
                UserId = userId,
                InvitatorId = invitatorId,
                IsReadOnly = isReadOnly
            };

            await _invitationRepository.CreateAsync(invitation);
            await _invitationRepository.SaveAsync();
        }

        public async Task AcceptInvatationAsync(int eventId, int userId)
        {
            var invitation = (await _invitationRepository.FindAllAsync(i => i.EventId == eventId && i.UserId == userId))
                .FirstOrDefault();

            // TODO: Replace with another exception.

            if (invitation == null)
                throw new NotFoundException();

            var calendarId = await _calendarService.GetUserDefaultCalendarIdAsync(userId);

            // TODO: Replace with another exception.

            if (calendarId == null)
                throw new NotFoundException();

            var eventCalendar = new EventCalendar
            {
                CalendarId = calendarId.Value,
                EventId = invitation.EventId,
                IsReadOnly = invitation.IsReadOnly
            };

            await _invitationRepository.RemoveAsync(invitation);
            await _eventCalendarRepository.CreateAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();
        }

        public async Task RejectInvitationAsync(int eventId, int userId)
        {
            var invitation = (await _invitationRepository.FindAllAsync(i => i.EventId == eventId && i.UserId == userId))
                .FirstOrDefault();

            // TODO: Replace with another exception.

            if (invitation == null)
                throw new NotFoundException();

            await _invitationRepository.RemoveAsync(invitation);
            await _invitationRepository.SaveAsync();
        }

        public async Task DeleteIntvitedUserAsync(int eventId, int userId)
        {
            var eventCalendar = (await _eventCalendarRepository.FindAllIncludingAsync(e => e.EventId == eventId && e.Calendar.OwnerId == userId,
                    e => e.Calendar))
                .FirstOrDefault();

            if (eventCalendar == null)
                throw new NotFoundException();

            await _eventCalendarRepository.RemoveAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();
        }

        public async Task UpdateInvitationReadOnlyAsync(int eventId, int userId, bool isReadOnly)
        {
            var invitation = (await _invitationRepository.FindAllAsync(i => i.EventId == eventId && i.UserId == userId))
                   .FirstOrDefault();

            // TODO: Replace with another exception.

            if (invitation == null)
                throw new NotFoundException();

            if (invitation.IsReadOnly == isReadOnly)
                throw new InvalidOperationException();

            invitation.IsReadOnly = isReadOnly;

            await _invitationRepository.UpdateAsync(invitation);
            await _invitationRepository.SaveAsync();
        }

        public async Task UpdateEventReadOnlyStateAsync(int eventId, int userId, bool isReadOnly)
        {
            var eventCalendar = (await _eventCalendarRepository.FindAllIncludingAsync(e => e.EventId == eventId && e.Calendar.OwnerId == userId, e => e.Calendar))
                .FirstOrDefault();

            // TODO: Replace with another exception.

            if (eventCalendar == null)
                throw new NotFoundException();

            if (eventCalendar.IsReadOnly == isReadOnly)
                throw new InvalidOperationException();

            eventCalendar.IsReadOnly = isReadOnly;

            await _eventCalendarRepository.UpdateAsync(eventCalendar);
            await _eventCalendarRepository.SaveAsync();
        }

        public async Task<bool> IsOwnerAsync(int eventId, int userId)
        {
            return (await _GetEventAsync(eventId)).OwnerId == userId;
        }

        public async Task<bool> IsOwnerOrInvitedAsync(int eventId, int userId)
        {
            var eventCalendars = (await _eventCalendarRepository
                .FindAllIncludingAsync(e => e.EventId == eventId, e => e.Calendar, e => e.Event))
                .AsEnumerable();

            return eventCalendars.Any(e => e.Calendar.OwnerId == userId && (!e.Event.IsPrivate || e.Event.OwnerId == userId));
        }

        // TODO: Union into one method.

        public async Task<bool> CanEditAsync(int eventId, int userId)
        {
            var eventCalendars = (await _eventCalendarRepository
                .FindAllIncludingAsync(e => e.EventId == eventId, e => e.Calendar, e => e.Event))
                .AsEnumerable();

            return eventCalendars.Any(e => e.Calendar.OwnerId == userId && !e.IsReadOnly && (!e.Event.IsPrivate || e.Event.OwnerId == userId));
        }

        #region Private methods.

        private async Task<Event> _GetEventAsync(int eventId, params Expression<Func<Event, object>>[] includeProperties)
        {
            Event ev;

            if (includeProperties.Any())
            {
                ev = (await _eventRepository.FindAllIncludingAsync(e => e.Id == eventId, includeProperties))
                    .FirstOrDefault();
            }
            else
            {
                ev = await _eventRepository.FindByIdAsync(eventId);
            }

            if (ev == null)
                throw new NotFoundException();

            return ev;
        }

        #endregion
    }
}