using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using AprioritWebCalendar.Infrastructure.Options;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_smtpOptions.FromTitle, _smtpOptions.Login));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpOptions.Server, _smtpOptions.Port, _smtpOptions.UseSsl);
                await client.AuthenticateAsync(_smtpOptions.Login, _smtpOptions.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
