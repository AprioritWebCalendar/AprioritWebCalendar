using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Business.DomainModels;
using AprioritWebCalendar.Business.Telegram;

namespace AprioritWebCalendar.Web.Jobs
{
    public class NotificationJob : IJob
    {
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;

        private readonly ILogger _logger;

        public NotificationJob(INotificationService notificationService, IEmailService emailService, ITelegramService telegramService, ILoggerFactory loggerFactory)
        {
            _notificationService = notificationService;
            _emailService = emailService;
            _telegramService = telegramService;

            _logger = loggerFactory.CreateLogger("NotificationJobLogger");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            IEnumerable<EventUser> eventUsers = null;

            _logger.LogInformation($"NotificationJob is running. UTC time: {DateTime.UtcNow}");

            try
            {
                eventUsers = await _notificationService.GetEventsToNotifyAsync(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get events to notify: {ex.Message}");
                return;
            }

            if (eventUsers?.Any() != true)
            {
                _logger.LogInformation($"No event-users to notify.");
                return;
            }

            _logger.LogInformation($"Event-Users to notify count: {eventUsers.Count()}");

            foreach (var e in eventUsers)
            {
                var message = CreateMessage(e);
                await SendEmailAsync(e, message);         

                if (e.User.IsTelegramNotificationEnabled == true)
                {
                    await SendTelegramMessageAsync(e, message);
                }
            }
        }

        private async Task SendEmailAsync(EventUser eventUser, string message)
        {
            try
            {
                await _emailService.SendAsync(eventUser.User.Email, eventUser.Event.Name, message);
                _logger.LogInformation($"Email to {eventUser.User.Email} has been sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to send Email ({eventUser.User.Email}), ex: {ex.Message}");
            }
        }

        private async Task SendTelegramMessageAsync(EventUser eventUser, string message)
        {
            try
            {
                await _telegramService.SendMessageAsync(eventUser.User.TelegramId.Value, message);
                _logger.LogInformation($"Telegram message to {eventUser.User.TelegramId} has been sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to send Telegram message ({eventUser.User.TelegramId}), ex: {ex.Message}");
            }
        }

        private string CreateMessage(EventUser eventUser)
        {
            return $"Don't forget about the event <b>{eventUser.Event.Name}.</b> It will start {GetUserDateTimeString(eventUser)}.";
        }

        private string GetUserDateTimeString(EventUser eventUser)
        {
            if (eventUser.Event.IsAllDay)
            {
                return $"in <b>{eventUser.Event.StartDate.Value.ToShortDateString()}</b>";
            }
            else
            {
                var dateTime = eventUser.User.TimeZone.ConvertFromUtc(eventUser.Event.StartDate.Value.Add(eventUser.Event.StartTime.Value));

                return $"in <b>{dateTime.ToShortDateString()}</b> at <b>{dateTime.ToShortTimeString()}</b>";
            }
        }
    }
}
