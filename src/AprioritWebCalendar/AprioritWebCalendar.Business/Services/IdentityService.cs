using System;
using System.Collections.Generic;
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

        public IdentityService(
            UserManager<ApplicationUser> userManager, 
            IRepository<ApplicationUser> userRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterRequestModel registerModel)
        {
            var user = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.UserName
            };

            return await _userManager.CreateAsync(user, registerModel.Password);
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _userRepository.FindByIdAsync(id);
            return _mapper.Map<User>(user);
        }
    }
}
