using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PharmacyBackend.Interface;
using PharmacyBackend.Models;

namespace PharmacyBackend.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtp = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.From = new MailAddress(_smtp.User);

            using var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                EnableSsl = _smtp.EnableSsl,
                Credentials = new NetworkCredential(_smtp.User, _smtp.Password)
            };

            await client.SendMailAsync(message);
        }
    }
}