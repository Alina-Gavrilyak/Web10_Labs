using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Contracts {
    public class EmailService : IEmailService {
        private readonly EmailServiceSettings settings;
        public EmailService(IOptions<EmailServiceSettings> settings) {
            this.settings = settings.Value;

        }
        public async Task SendEmailAsync(string email, string subject, string message) {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Turnover Api", settings.SmtpUser));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                Text = message
            };

            using (var client = new SmtpClient()) {
                await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, true);
                await client.AuthenticateAsync(settings.SmtpUser, settings.SmtpPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }

    public class EmailServiceSettings {
        public const string SectionKey = "EmailServiceSettings";

        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
