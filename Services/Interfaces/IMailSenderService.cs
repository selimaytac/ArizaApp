using System.Collections.Generic;
using System.Threading.Tasks;
using ArizaApp.Models.Entities;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace ArizaApp.Services.Interfaces
{
    public interface IMailSenderService
    {
        Task SendEmailAsync(List<string> mailAddresses, string subject, ArizaModel mailModel, IList<IFormFile> attachments = null);
        MimeEntity CreateArizaHtmlEmailBody(ArizaModel mailModel, IList<IFormFile> attachments = null);
    }
}