using AutoMapper;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.ViewModel.Calendar;

namespace AprioritWebCalendar.Bootstrap
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region User.

            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed));

            CreateMap<User, UserViewModel>();

            #endregion

            #region Calendar.

            CreateMap<Business.DomainModels.Calendar, Data.Models.Calendar>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore());

            CreateMap<Data.Models.Calendar, Business.DomainModels.Calendar>();

            CreateMap<CalendarShortModel, Business.DomainModels.Calendar>();
            CreateMap<Business.DomainModels.Calendar, CalendarShortModel>();

            #endregion

            #region Event.

            CreateMap<Business.DomainModels.Event, Data.Models.Event>();
            CreateMap<Data.Models.Event, Business.DomainModels.Event>();

            #endregion

            #region UserCalendar.

            CreateMap<Business.DomainModels.UserCalendar, Data.Models.UserCalendar>();
            CreateMap<Data.Models.UserCalendar, Business.DomainModels.UserCalendar>();

            CreateMap<Business.DomainModels.UserCalendar, UserCalendarViewModel>();

            #endregion

            #region EventCalendar.

            CreateMap<Business.DomainModels.EventCalendar, Data.Models.EventCalendar>();
            CreateMap<Data.Models.EventCalendar, Business.DomainModels.EventCalendar>();

            #endregion

            #region Invitation.

            CreateMap<Business.DomainModels.Invitation, Data.Models.Invitation>();
            CreateMap<Data.Models.Invitation, Business.DomainModels.Invitation>();

            #endregion

            #region Period.

            CreateMap<Business.DomainModels.Period, Data.Models.Period>();
            CreateMap<Data.Models.Period, Business.DomainModels.Period>();

            #endregion
        }
    }
}
