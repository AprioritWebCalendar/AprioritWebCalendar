using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ICaptchaService _captchaService;
        private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;

        public AccountController(
            IIdentityService identityService, 
            IUserAuthenticationService userAuthenticationService, 
            ICaptchaService captchaService,
            IMapper mapper,
            IOptions<JwtOptions> jwtOptions)
        {
            _identityService = identityService;
            _userAuthenticationService = userAuthenticationService;
            _captchaService = captchaService;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _identityService.GetUserAsync(this.GetUserId());
            return Ok(_mapper.Map<UserViewModel>(user));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateApiModelFilter]
        public async Task<IActionResult> Login([FromBody]LoginRequestModel model)
        {
            if (User.Identity.IsAuthenticated)
                return Forbid();

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
        [AllowAnonymous]
        [ValidateApiModelFilter]
        public async Task<IActionResult> Register([FromBody]RegisterRequestModel model)
        {
            if (User.Identity.IsAuthenticated)
                return Forbid();

            if (!await _captchaService.TryVerifyCaptchaAsync(model.RecaptchaToken))
                return this.BadRequestError("Invalid captcha.");

            var registerResult = await _identityService.CreateUserAsync(model);

            if (registerResult == IdentityResult.Success)
                return Ok();

            return BadRequest(registerResult.Errors.ToStringEnumerable());
        }
    }
}
