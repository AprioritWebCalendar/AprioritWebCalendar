using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Infrastructure.DataTypes;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.Business.Telegram;
using AprioritWebCalendar.Infrastructure.Exceptions;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Settings")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly ITelegramService _telegramService;
        private readonly ITelegramVerificationService _telegramVerificationService;
        private readonly IIdentityService _identityService;

        public SettingsController(
            ISettingsService settingsService,
            ITelegramService telegramService,
            ITelegramVerificationService telegramVerificationService, 
            IIdentityService identityService)
        {
            _settingsService = settingsService;
            _telegramService = telegramService;
            _telegramVerificationService = telegramVerificationService;
            _identityService = identityService;
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

        [HttpPost("Telegram")]
        public async Task<IActionResult> ConnectAccount([FromBody]string code)
        {
            try
            {
                var telegramId = await _telegramVerificationService.TryVerifyAsync(User.GetUserId(), code);
                await _telegramService.SendMessageAsync(telegramId, $"Your Telegram account has been connected to <b>{User.Identity.Name}</b> profile successfully.");
                return Ok(new { TelegramId = telegramId });
            }
            catch (NotFoundException)
            {
                return this.BadRequestError("The verification code is invalid.");
            }
        }

        [HttpPost("Telegram/Notifications")]
        public async Task<IActionResult> TelegramNotificationsEnabled([FromBody]bool isEnabled)
        {
            var user = await _identityService.GetUserAsync(User.GetUserId());
            string message = null;

            if (isEnabled)
            {
                await _identityService.EnableTelegramNotificationsAsync(user.Id);
                message = "Notifications have been enabled successfully.";
            }
            else
            {
                await _identityService.DisableTelegramNotificationsAsync(user.Id);
                message = "Notifications have been disabled successfully.";
            }

            await _telegramService.SendMessageAsync(user.TelegramId.Value, message);
            return Ok();
        }

        [HttpPost("Telegram/Reset")]
        public async Task<IActionResult> TelegramReset()
        {
            var user = await _identityService.GetUserAsync(User.GetUserId());
            await _identityService.ResetTelegramIdAsync(user.Id);

            await _telegramService.SendMessageAsync(user.TelegramId.Value, $"Your Telegram account has been disconnected from profile <b>{user.UserName}</b>");
            return Ok();
        }
    }
}
