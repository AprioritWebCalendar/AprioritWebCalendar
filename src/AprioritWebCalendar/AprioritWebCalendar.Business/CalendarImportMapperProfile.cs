using AutoMapper;

namespace AprioritWebCalendar.Business
{
    public class CalendarImportMapperProfile : Profile
    {
        public CalendarImportMapperProfile()
        {
            CreateMap<DomainModels.Calendar, Data.Models.Calendar>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.SharedUsers, opt => opt.Ignore());

            CreateMap<DomainModels.EventCalendar, Data.Models.EventCalendar>();

            CreateMap<DomainModels.Event, Data.Models.Event>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.Invitations, opt => opt.Ignore())
                .ForMember(dest => dest.Calendars, opt => opt.Ignore())
                .ForMember(dest => dest.LocationDescription, opt => opt.MapFrom(src => src.Location.Description))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude))
                .ForMember(dest => dest.Lattitude, opt => opt.MapFrom(src => src.Location.Lattitude));

            CreateMap<DomainModels.Period, Data.Models.Period>();
        }
    }
}
