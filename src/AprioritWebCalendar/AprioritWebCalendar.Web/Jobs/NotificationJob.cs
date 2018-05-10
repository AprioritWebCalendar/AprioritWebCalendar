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
                var userLocalTime = e.Event.StartDate.Value.ToShortDateString();

                if (!e.Event.IsAllDay)
                {
                    userLocalTime = e.User.TimeZone.ConvertFromUtc(e.Event.StartDate.Value.Add(e.Event.StartTime.Value)).ToString("F");
                }

                var message = $"Don't forget about the event <b>{e.Event.Name}.</b> It will start at <b>{userLocalTime}</b>.";

                try
                {
                    await _emailService.SendAsync(e.User.Email, e.Event.Name, message);
                    _logger.LogInformation($"Email to {e.User.Email} has been sent.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unable to send Email ({e.User.Email}), ex: {ex.Message}");
                }

                if (e.User.IsTelegramNotificationEnabled == true)
                {
                    try
                    {
                        await _telegramService.SendMessageAsync(e.User.TelegramId.Value, message);
                        _logger.LogInformation($"Telegram message to {e.User.TelegramId} has been sent.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unable to send Telegram message ({e.User.TelegramId}), ex: {ex.Message}");
                    }
                }
            }
        }
    }
}
