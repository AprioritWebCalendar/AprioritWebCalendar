using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.ViewModel.Account;
using AprioritWebCalendar.Infrastructure.Exceptions;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Infrastructure.Options;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public AccountController(
            IIdentityService identityService, 
            IUserAuthenticationService userAuthenticationService, 
            IMapper mapper,
            IOptions<JwtOptions> jwtOptions)
        {
            _identityService = identityService;
            _userAuthenticationService = userAuthenticationService;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("Login")]
        [OnlyAnonymous]
        [ValidateApiModelFilter]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        {
            try
            {
                var user = await _userAuthenticationService.FindUserByCredentialsAsync(model);
                var token = _userAuthenticationService.CreateJwtToken(_userAuthenticationService.CreateClaims(user), _jwtOptions);

                var response = new
                {
                    AccessToken = token,
                    User = _mapper.Map<UserViewModel>(user)
                };
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return this.BadRequestError("Invalid login data.");
            }
        }

        [HttpPost("Register")]
        [OnlyAnonymous]
        [ValidateApiModelFilter]
        public async Task<IActionResult> Register([FromBody]RegisterRequestModel model)
        {
            var registerResult = await _identityService.CreateUserAsync(model);

            if (registerResult == IdentityResult.Success)
                return Ok();

            return BadRequest(registerResult.Errors.ToStringEnumerable());
        }
    }
}
