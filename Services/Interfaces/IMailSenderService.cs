using System.Collections.Generic;
using System.Threading.Tasks;
using ArizaApp.Models.Entities;
using MimeKit;

namespace ArizaApp.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(List<string> mailAddresses, string subject, ArizaModel mailModel);
        MimeEntity CreateArizaHtmlEmailBody(ArizaModel mailModel);
    }
}