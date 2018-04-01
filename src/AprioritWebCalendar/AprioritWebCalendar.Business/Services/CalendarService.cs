using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.Exceptions;
using AprioritWebCalendar.ViewModel.Calendar;
using DomainCalendar = AprioritWebCalendar.Business.DomainModels.Calendar;
using DomainUserCalendar = AprioritWebCalendar.Business.DomainModels.UserCalendar;

namespace AprioritWebCalendar.Business.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IRepository<UserCalendar> _userCalendarRepository;

        private readonly IMapper _mapper;

        public CalendarService(
            IRepository<Calendar> calendarRepository,
            IRepository<UserCalendar> userCalendarRepository,
            IMapper mapper)
        {
            _calendarRepository = calendarRepository;
            _userCalendarRepository = userCalendarRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DomainCalendar>> GetCalendarsAsync(int userId, bool onlyOwn = false)
        {
            Expression<Func<Calendar, bool>> predicate;

            if (onlyOwn)
                predicate = c => c.OwnerId == userId;
            else
                predicate = c => c.OwnerId == userId || c.SharedUsers.Any(u => u.UserId == userId);

            var calendars = await _calendarRepository.FindAllIncludingAsync(predicate,
                c => c.SharedUsers);

            return _mapper.Map<IEnumerable<DomainCalendar>>(calendars);
        }

        public async Task<DomainCalendar> GetCalendarByIdAsync(int calendarId)
        {
            var calendar = await _GetByIdAsync(calendarId);
            return _mapper.Map<DomainCalendar>(calendar);
        }

        public async Task<DomainCalendar> CreateCalendarAsync(CalendarShortModel createModel, int ownerId)
        {
            var calendar = _mapper.Map<Calendar>(createModel);
            calendar.OwnerId = ownerId;

            calendar = await _calendarRepository.CreateAsync(calendar);
            await _calendarRepository.SaveAsync();

            return _mapper.Map<DomainCalendar>(calendar);
        }

        public async Task<DomainCalendar> UpdateCalendarAsync(int calendarId, CalendarShortModel updateModel)
        {
            var calendar = await _GetByIdAsync(calendarId);
            calendar = _mapper.Map<Calendar>(updateModel);

            await _calendarRepository.UpdateAsync(calendar);
            await _calendarRepository.SaveAsync();

            return _mapper.Map<DomainCalendar>(calendar);
        }

        public async Task DeleteCalendarAsync(int calendarId)
        {
            var calendar = await _GetByIdAsync(calendarId);
            await _calendarRepository.RemoveAsync(calendar);
            await _calendarRepository.SaveAsync();
        }

        public async Task ShareCalendarAsync(int calendarId, int userId, bool isReadOnly = true)
        {
            // TODO: Replace for custom exceptions.

            var calendar = await _GetByIdAsync(calendarId, c => c.SharedUsers);

            if (calendar.OwnerId == userId)
                throw new InvalidOperationException();

            if (calendar.SharedUsers.Any(u => u.UserId == userId))
                throw new InvalidOperationException();

            await _userCalendarRepository.CreateAsync(new UserCalendar
            {
                CalendarId = calendarId,
                UserId = userId,
                IsReadOnly = isReadOnly
            });
            await _userCalendarRepository.SaveAsync();
        }

        public async Task RemoveSharingAsync(int calendarId, int userId)
        {
            var userCalendar = await _GetUserCalendarAsync(userId, calendarId);

            await _userCalendarRepository.RemoveAsync(userCalendar);
            await _userCalendarRepository.SaveAsync();
        }

        public async Task SubscribeCalendarAsync(int calendarId, int userId)
        {
            // TODO: Replace for custom exceptions.

            var userCalendar = await _GetUserCalendarAsync(userId, calendarId);

            if (userCalendar.IsSubscribed)
                throw new InvalidOperationException();

            userCalendar.IsSubscribed = true;

            await _userCalendarRepository.UpdateAsync(userCalendar);
            await _userCalendarRepository.SaveAsync();
        }

        public async Task UnsunscribeCalendarAsync(int calendarId, int userId)
        {
            // TODO: Replace for custom exceptions.

            var userCalendar = await _GetUserCalendarAsync(userId, calendarId);

            if (!userCalendar.IsSubscribed)
                throw new InvalidOperationException();

            userCalendar.IsSubscribed = false;

            await _userCalendarRepository.UpdateAsync(userCalendar);
            await _userCalendarRepository.SaveAsync();
        }

        public async Task<IEnumerable<DomainUserCalendar>> GetUsersSharedWithAsync(int calendarId)
        {
            var users = await _userCalendarRepository.FindAllIncludingAsync(c => c.CalendarId == calendarId,
                c => c.User);

            return _mapper.Map<IEnumerable<DomainUserCalendar>>(users);
        }

        public async Task<bool> IsOwnerAsync(int calendarId, int userId)
        {
            var calendar = await _GetByIdAsync(calendarId);
            return calendar.OwnerId == userId;
        }

        #region Private methods.

        private async Task<Calendar> _GetByIdAsync(int id, params Expression<Func<Calendar, object>>[] includeProperties)
        {
            var calendar = (await _calendarRepository.FindAllIncludingAsync(c => c.Id == id, includeProperties))
                .FirstOrDefault();

            if (calendar == null)
                throw new NotFoundException();

            return calendar;
        }

        private async Task<UserCalendar> _GetUserCalendarAsync(int userId, int calendarId)
        {
            // TODO: Replace for custom exception.
            var userCalendar = await _userCalendarRepository.FindByKeysAsync(userId, calendarId);

            if (userCalendar == null)
                throw new InvalidOperationException();

            return userCalendar;
        }

        #endregion
    }
}
