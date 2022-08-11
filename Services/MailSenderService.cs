using System.Collections.Generic;
using System.Linq;
using ArizaApp.Models.Entities;
using ArizaApp.Models.Options;
using ArizaApp.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ArizaApp.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly MailOptions _mailOptions;

        public MailSenderService(IOptions<MailOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
        }

        public bool SendBulkArizaMail(List<string> mailAddresses, string subject, ArizaModel mailModel)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _mailOptions.Sender));
            emailMessage.To.AddRange(mailAddresses.Select(x => new MailboxAddress("email",x)));
            emailMessage.Subject = subject;
            // emailMessage.Body = new BodyBuilder().HtmlBody();
            // TODO: add template
            return true;
        }
    }
}