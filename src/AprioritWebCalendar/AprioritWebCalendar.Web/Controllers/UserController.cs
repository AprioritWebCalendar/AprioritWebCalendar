using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Web.Filters;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ExceptionHandler]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public UserController(IMapper mapper, IIdentityService identityService)
        {
            _mapper = mapper;
            _identityService = identityService;
        }

        [HttpGet("{emailOrUserName}")]
        public async Task<IActionResult> Get(string emailOrUserName)
        {
            if (emailOrUserName?.Length < 4)
                return this.BadRequestError("\"emailOrUserName\" must be a string more than 3 symbols.");

            var users = await _identityService.FindUsersAsync(emailOrUserName, this.GetUserId());
            return this.OkOrNoContent(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }
    }
}
