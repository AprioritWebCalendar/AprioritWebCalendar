using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Business.Telegram;
using AprioritWebCalendar.Web.SignalR.Telegram;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.ViewModel.Event;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.Infrastructure.DataTypes;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Telegram")]
    public class TelegramController : Controller
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramVerificationService _telegramVerificationService;
        private readonly IIdentityService _identityService;
        private readonly IEventService _eventService;
        private readonly TelegramHubManager _telegramHubManager;

        private readonly IMapper _mapper;

        private TelegramMessageUpdate _update;
        private int _telegramId;

        private readonly Dictionary<string, Func<Task<IActionResult>>> _responses = new Dictionary<string, Func<Task<IActionResult>>>();

        public TelegramController(
            ITelegramService telegramService,
            ITelegramVerificationService telegramVerificationService,
            IIdentityService identityService,
            IEventService eventService,
            TelegramHubManager telegramHubManager,
            IMapper mapper)
        {
            _telegramService = telegramService;
            _telegramVerificationService = telegramVerificationService;
            _identityService = identityService;
            _eventService = eventService;
            _telegramHubManager = telegramHubManager;
            _mapper = mapper;

            _responses.Add("start", _Start);
            _responses.Add("connect", _Connect);
            _responses.Add("reset", _Reset);
            _responses.Add("pause", _Pause);
            _responses.Add("restore", _Restore);

            _responses.Add("today", _Today);
            _responses.Add("tomorrow", _Tomorrow);
        }

        [HttpPost("{token}")]
        [ServiceFilter(typeof(ValidateTelegramTokenAttribute))]
        [ValidateTelegramUpdate]
        public async Task<IActionResult> WebHook(string token, [FromBody]TelegramMessageUpdate update)
        {
            _update = update;
            _telegramId = update.Message.From.Id;

            return await _responses[update.Message.GetCommandWithoutSlash()]();
        }

        private async Task<IActionResult> _Today()
        {
            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user == null)
                return Ok();

            var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, user.TimeZone.ToWindowsTimeZoneInfo());
            var events = await _eventService.GetEventsByDateAsync(user.Id, date.Date);

            return await _ReturnEventsByDate(events, user.TimeZone);
        }

        private async Task<IActionResult> _Tomorrow()
        {
            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user == null)
                return Ok();

            var date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, user.TimeZone.ToWindowsTimeZoneInfo());
            date = date.AddDays(1);
            var events = await _eventService.GetEventsByDateAsync(user.Id, date.Date);

            return await _ReturnEventsByDate(events, user.TimeZone);
        }

        private async Task<IActionResult> _ReturnEventsByDate(IEnumerable<Event> events, TimeZoneInfoIana timeZone)
        {
            if (!events.Any())
                return await _SendMessageResponse("No events");

            var viewModels = _mapper.Map<IEnumerable<EventViewModel>>(events);
            var message = string.Join(string.Empty, viewModels.Select(e => e.ToMessageString(timeZone)));
            return await _SendMessageResponse(message);
        }

        #region Simple commands.

        private async Task<IActionResult> _Connect()
        {
            if (await _telegramVerificationService.CodeExistsAsync(_telegramId))
                await _telegramVerificationService.RemoveCodesAsync(_telegramId);

            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user != null)
            {
                return await _SendMessageResponse($"Your Telegram account is already connected with user <b>{user.UserName}</b>.");
            }
            else
            {
                var code = await _telegramVerificationService.GetVerificationCodeAsync(_telegramId);

                var message = "Use this token to connect with account on the site.\nOpen <b>Settings</b> > <b>Telegram</b> and put the code in the field and press <b>Connect</b>\n\n"
                                +"If everything is OK, the account will be connected. Do not show the message with code to anybody.\n\n"
                                + "If you change one's mind and do not want to connect account, I advice you to delete the message with the code.";

                await _SendMessage(message);
                return await _SendMessageResponse($"<code>{code}</code>");
            }
        }

        private async Task<IActionResult> _Pause()
        {
            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user == null)
                return await _SendMessageResponse("Your Telegram account is not connected with any profile.");

            if (user.IsTelegramNotificationEnabled == false)
                return await _SendMessageResponse("Notifications are already disabled.");

            await _identityService.DisableTelegramNotificationsAsync(user.Id);
            await _telegramHubManager.TelegramNotificationsDisabledAsync(user.Id);
            return await _SendMessageResponse("Notifications have been disabled successfully.");
        }

        private async Task<IActionResult> _Restore()
        {
            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user == null)
                return await _SendMessageResponse("Your Telegram account is not connected with any profile.");

            if (user.IsTelegramNotificationEnabled == true)
                return await _SendMessageResponse("Notifications are already enabled.");

            await _identityService.EnableTelegramNotificationsAsync(user.Id);
            await _telegramHubManager.TelegramNotificationsEnabledAsync(user.Id);
            return await _SendMessageResponse("Notifications have been enabled successfully.");
        }

        private async Task<IActionResult> _Reset()
        {
            string message = null;
            var user = await _identityService.GetByTelegramIdAsync(_telegramId);

            if (user == null)
            {
                message = "Your Telegram account is not connected with any profile.";
            }
            else
            {
                await _identityService.ResetTelegramIdAsync(user.Id);
                await _telegramHubManager.TelegramResetedAsync(user.Id);
                message = $"Your Telegram account has been disconnected from profile <b>{user.UserName}</b>.";
            }

            return await _SendMessageResponse(message);
        }

        private async Task<IActionResult> _Start()
        {
            var message = "Hello, I'm <b>Web Calendar Bot</b>. I can send notifications about nearest events in your calenadr.\nBut... "
                            + "You have to connect your Telegram account with account from the site.\n"
                            + "Run <a>/connect</a> command and follow the promts.";
            return await _SendMessageResponse(message);
        }

        #endregion

        private async Task<IActionResult> _SendMessageResponse(string message)
        {
            await _SendMessage(message);
            return Ok();
        }

        private async Task _SendMessage(string message)
        {
            await _telegramService.SendMessageAsync(_telegramId, message);
        }
    }
}
