using AutoMapper;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.ViewModel.Account;

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
        }
    }
}
