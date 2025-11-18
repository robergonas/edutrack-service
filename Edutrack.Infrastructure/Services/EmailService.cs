using EduTrack.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;
        public EmailService(IConfiguration config)
        {
            _fromAddress = config["Email:From"];
            _smtpClient = new SmtpClient(config["Email:SmtpHost"])
            {
                Port = int.Parse(config["Email:SmtpPort"]),
                Credentials = new NetworkCredential(config["Email:Username"], config["Email:Password"]),
                EnableSsl = true
            };
        }
        public async Task SendEmailAsync(EmailMessage message)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_fromAddress),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            foreach (var to in message.To)
                mail.To.Add(to);

            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    var stream = new MemoryStream(attachment.Content);
                    mail.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.ContentType));
                }
            }

            await _smtpClient.SendMailAsync(mail);
        }
    }
}
