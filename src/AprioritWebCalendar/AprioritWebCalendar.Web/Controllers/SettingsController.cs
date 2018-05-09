using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Infrastructure.DataTypes;
using AprioritWebCalendar.Web.Extensions;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Settings")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("TimeZone")]
        public async Task<IActionResult> GetTimeZone()
        {
            var tz = await _settingsService.GetTimeZoneAsync(User.GetUserId());
            return Ok(new { TimeZone = tz.ToString() });
        }

        [HttpPost("TimeZone")]
        public async Task<IActionResult> SaveTimeZone([FromBody]string timeZone)
        {
            if (!TimeZoneInfoIana.IsValidTimeZoneName(timeZone))
                return this.BadRequestError("Time Zone name must be IANA TimeZone ID.");

            await _settingsService.SetTimeZoneAsync(User.GetUserId(), new TimeZoneInfoIana(timeZone));
            return Ok();
        }
    }
}
