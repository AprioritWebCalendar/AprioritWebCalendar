using AutoMapper;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.ViewModel.Calendar;
using AprioritWebCalendar.ViewModel.Event;

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
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.EventCalendars, opt => opt.Ignore())
                .ForMember(dest => dest.SharedUsers, opt => opt.Ignore());

            CreateMap<Data.Models.Calendar, Business.DomainModels.Calendar>();

            CreateMap<CalendarRequestModel, Business.DomainModels.Calendar>();
            CreateMap<Business.DomainModels.Calendar, CalendarRequestModel>();

            CreateMap<Business.DomainModels.Calendar, CalendarViewModel>();

            #endregion

            #region Event.

            CreateMap<Business.DomainModels.Event, Data.Models.Event>()
                .ForMember(dest => dest.LocationDescription, opt => opt.MapFrom(src => src.Location.Description))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude))
                .ForMember(dest => dest.Lattitude, opt => opt.MapFrom(src => src.Location.Lattitude))

                .ForMember(dest => dest.Calendars, opt => opt.Ignore())
                .ForMember(dest => dest.Invitations, opt => opt.Ignore())
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore());
                //.ForMember(dest => dest.Period, opt => opt.Ignore());

            CreateMap<Data.Models.Event, Business.DomainModels.Event>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location{ Description = src.LocationDescription, Lattitude = src.Lattitude, Longitude = src.Longitude}));


            CreateMap<EventRequestModel, Business.DomainModels.Event>();

            #endregion

            #region Location.

            CreateMap<Location, LocationViewModel>();
            CreateMap<LocationViewModel, Location>();

            #endregion

            #region UserCalendar.

            CreateMap<Business.DomainModels.UserCalendar, Data.Models.UserCalendar>()
                .ForMember(dest => dest.Calendar, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());


            CreateMap<Data.Models.UserCalendar, Business.DomainModels.UserCalendar>();

            CreateMap<Business.DomainModels.UserCalendar, UserCalendarViewModel>();

            #endregion

            #region EventCalendar.

            CreateMap<Business.DomainModels.EventCalendar, Data.Models.EventCalendar>()
                .ForMember(dest => dest.Calendar, opt => opt.Ignore())
                .ForMember(dest => dest.Event, opt => opt.Ignore());

            CreateMap<Data.Models.EventCalendar, Business.DomainModels.EventCalendar>();

            #endregion

            #region Invitation.

            CreateMap<Business.DomainModels.Invitation, Data.Models.Invitation>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.Invitator, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Data.Models.Invitation, Business.DomainModels.Invitation>();

            #endregion

            #region Period.

            CreateMap<Business.DomainModels.Period, Data.Models.Period>()
                .ForMember(dest => dest.EventId, opt => opt.Ignore());

            CreateMap<Data.Models.Period, Business.DomainModels.Period>();

            CreateMap<Business.DomainModels.Period, PeriodViewModel>();
            CreateMap<PeriodViewModel, Business.DomainModels.Period>();

            #endregion
        }
    }
}
