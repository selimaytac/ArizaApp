using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.Models.Entities;
using ArizaApp.Models.Options;
using ArizaApp.Services.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ArizaApp.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly MailOptions _mailOptions;

        public MailSenderService(IOptions<MailOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
        }


        public async Task SendEmailAsync(List<string> mailAddresses, string subject, ArizaModel mailModel)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailOptions.Sender);
            email.To.AddRange(mailAddresses.Select(MailboxAddress.Parse));
            email.Subject = subject;
            email.Body = CreateArizaHtmlEmailBody(mailModel);

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public MimeEntity CreateArizaHtmlEmailBody(ArizaModel mailModel)
        {
            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\mailTemplates\\");
            var templateName = mailModel.State == "Bitti" ? "ArizaBitis.txt" : "ArizaBaslangic.txt";
            var templatePath = System.IO.Path.Combine(path + templateName);

            var str = new StreamReader(templatePath);
            var mailText = str.ReadToEnd();

            mailText =
                mailText.Replace("#{FaultNo}#", mailModel.FaultNo)
                    .Replace("#{FaultType}#", mailModel.FaultType)
                    .Replace("#{State}#", mailModel.State)
                    .Replace("#{Priority}#", mailModel.Priority)
                    .Replace("#{Description}#", mailModel.Description)
                    .Replace("#{StartDate}#", mailModel.StartDate.ToShortDateString())
                    .Replace("#{FailureCause}#", mailModel.FailureCause)
                    .Replace("#{AlarmStatus}#", mailModel.AlarmStatus ? "Evet" : "Hayır")
                    .Replace("#{NotifiedBy}#", mailModel.NotifiedBy)
                    .Replace("#{AffectedServices}#", mailModel.AffectedServices)
                    .Replace("#{AffectedFirms}#", mailModel.AffectedFirms);

            if (mailModel.State == "Bitti")
                mailText = mailText.Replace("#{EndDate}#", mailModel.EndDate.ToShortDateString());

            var builder = new BodyBuilder
            {
                HtmlBody = mailText
            };
            
            return builder.ToMessageBody();
        }
    }
}