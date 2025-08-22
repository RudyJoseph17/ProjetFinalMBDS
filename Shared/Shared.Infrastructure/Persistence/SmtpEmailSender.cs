using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.ApplicationUsers;

namespace Shared.Infrastructure.Persistence
{
    public class SmtpEmailSender: IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public SmtpEmailSender(IConfiguration config)
        {
            _smtpServer = config["SMTP:Server"];
            _smtpPort = int.Parse(config["SMTP:Port"]);
            _smtpUser = config["SMTP:User"];
            _smtpPass = config["SMTP:Pass"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_smtpUser, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
