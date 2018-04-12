using System.Collections.Generic;
using System.Linq;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.ViewModel.Calendar;
using AutoMapper;

namespace AprioritWebCalendar.Web.Extensions
{
    /// <summary>
    /// These extensions are to map from Calendar (domain) to CalendarViewModels.
    /// Mapping these types, we have to take into account a user, which gets calendars.
    /// Because CalendarViewModel.IsReadOnly and IsSubscribed depend on the user.
    /// </summary>
    public static class MapperExtensions
    {
        public static IEnumerable<CalendarViewModel> MapToCalendarViewModel(this IMapper mapper, IList<Calendar> calendars, int currentUserId)
        {
            var viewModels = mapper.Map<IList<CalendarViewModel>>(calendars);

            for (var i = 0; i < viewModels.Count(); i++)
            {
                viewModels[i].IsReadOnly = calendars[i].SharedUsers.FirstOrDefault(u => u.UserId == currentUserId)?.IsReadOnly;
                viewModels[i].IsSubscribed = calendars[i].SharedUsers.FirstOrDefault(u => u.UserId == currentUserId)?.IsSubscribed;
            }

            return viewModels;
        }

        public static CalendarViewModel MapToCalendarViewModel(this IMapper mapper, Calendar calendar, int currentUserId)
        {
            var viewModel = mapper.Map<CalendarViewModel>(calendar);

            viewModel.IsReadOnly = calendar.SharedUsers.FirstOrDefault(u => u.UserId == currentUserId)?.IsReadOnly;
            viewModel.IsSubscribed = calendar.SharedUsers.FirstOrDefault(u => u.UserId == currentUserId)?.IsSubscribed;

            return viewModel;
        }
    }
}
