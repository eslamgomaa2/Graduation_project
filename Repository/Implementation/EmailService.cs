using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OA.Domain.Settings;
using OA.Service.Contract;
using System;
using System.Threading.Tasks;

namespace OA.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailsettings;

        public EmailService(IOptions<MailSettings> mailsettings)
        {
            _mailsettings = mailsettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailsettings.Email),
                Subject = subject,
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailsettings.DisplayName, _mailsettings.Email));

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignore SSL certificate validation errors
            
            smtp.Connect(_mailsettings.Host, 465, SecureSocketOptions.Auto);

            smtp.Authenticate(_mailsettings.Email, _mailsettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
