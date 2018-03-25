using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.Options;
using AprioritWebCalendar.Infrastructure.Exceptions;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.Business.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserAuthenticationService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<User> FindUserByCredentialsAsync(LoginRequestModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.EmailOrUserName)
                ?? await _userManager.FindByNameAsync(loginModel.EmailOrUserName);

            if (user == null)
                throw new NotFoundException();

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
                throw new NotFoundException();

            var domainUser = _mapper.Map<User>(user);
            domainUser.Roles = await _userManager.GetRolesAsync(user);
            return domainUser;
        }

        public ClaimsIdentity CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var r in user.Roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r));
            }

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public string CreateJwtToken(ClaimsIdentity claims, JwtOptions jwtOptions)
        {
            var utcNow = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    notBefore: utcNow,
                    claims: claims.Claims,
                    expires: utcNow.AddMinutes(jwtOptions.Lifetime),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key)), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
