using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Contracts
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceSettings settings;
        public EmailService(IOptions<EmailServiceSettings> settings)
        {
            this.settings = settings.Value;

        }
        public void SendEmail(string email, string subject, string message)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                    X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            MailMessage mailMessage = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = message,
                From = new MailAddress(settings.SmtpUser)
            };
            mailMessage.To.Add(email);

            SmtpClient smtp = new SmtpClient()
            {
                Host = settings.SmtpHost,
                Port = settings.SmtpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(settings.SmtpUser, settings.SmtpPassword),
            };

            smtp.Send(mailMessage);
        }
    }

    public class EmailServiceSettings
    {
        public const string SectionKey = "EmailServiceSettings";

        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
