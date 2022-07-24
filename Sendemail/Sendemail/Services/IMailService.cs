using System;
using System.Threading.Tasks;
using Sendemail.Models;

namespace Sendemail.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
