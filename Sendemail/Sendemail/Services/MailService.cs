using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sendemail.Models;
using Sendemail.Settings;
using Sendemail.Services;
using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Sendemail.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
            _mailSettings.Mail = "mail@gmail.com";
            _mailSettings.Password = "YourPassword";
            _mailSettings.Host = "smtp.gmail.com";
            _mailSettings.DisplayName = "Your Name";
            _mailSettings.Port = 587;
        }

        /*public Task SendEmailAsync(MailRequest mailRequest)
        {
            throw new NotImplementedException();
        }*/

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            mailRequest.ToEmail = "mail@hotmail.com";
            mailRequest.Subject = "Test";
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
