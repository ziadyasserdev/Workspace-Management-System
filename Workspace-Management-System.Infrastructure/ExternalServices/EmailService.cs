using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.ExternalServices;
using Workspace_Management_System.Application.Settings;

namespace Workspace_Management_System.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
