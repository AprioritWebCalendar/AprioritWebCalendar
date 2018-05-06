using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Data.Interfaces;

namespace AprioritWebCalendar.Business.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
        private readonly ICalendarService _calendarService;

        public IdentityService(
            UserManager<ApplicationUser> userManager, 
            IRepository<ApplicationUser> userRepository,
            IMapper mapper,
            ICalendarService calendarService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _calendarService = calendarService;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterRequestModel registerModel)
        {
            var user = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result != IdentityResult.Success)
                return result;

            await _calendarService.CreateDefaultCalendarAsync(user.Id, user.UserName);
            return result;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            return _mapper.Map<User>(user);
        }

        public async Task<IEnumerable<User>> FindUsersAsync(string emailOrUserName, int currentUserId)
        {
            Expression<Func<ApplicationUser, bool>> filter = u => u.Id != currentUserId
                && (u.Email.IndexOf(emailOrUserName) >= 0 || u.UserName.IndexOf(emailOrUserName) >= 0);

            var users = await _userRepository.FindAllAsync(filter);
            return _mapper.Map<IEnumerable<User>>(users);
        }
    }
}
