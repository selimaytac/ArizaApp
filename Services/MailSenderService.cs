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
            email.Bcc.AddRange(mailAddresses.Select(MailboxAddress.Parse));
            email.Subject = subject;
            email.Body = CreateArizaHtmlEmailBody(mailModel);

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.None);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public MimeEntity CreateArizaHtmlEmailBody(ArizaModel mailModel)
        {
            var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\mailTemplates\\");
            var templateName = string.Empty;

            if (mailModel.FaultType == "Arıza")
                templateName = mailModel.State is "Bitti" or "Yaşanmıştır" ? "ArizaBitis.txt" : "ArizaBaslangic.txt";
            else
                templateName = "PlanliCalisma.txt";

            var templatePath = System.IO.Path.Combine(path + templateName);

            var str = new StreamReader(templatePath, System.Text.Encoding.UTF8);
            var mailText = str.ReadToEnd();

            if(mailModel.FaultType == "Arıza")
                mailText = ReplaceArizaBody(mailText, mailModel);
            else
                mailText = ReplacePlanliCalismaBody(mailText, mailModel);            
            var builder = new BodyBuilder
            {
                HtmlBody = mailText
            };

            return builder.ToMessageBody();
        }

        private string ReplacePlanliCalismaBody(string mailText, ArizaModel mailModel)
        {
            return mailText.Replace("#{FaultNo}#", mailModel.FaultNo)
                .Replace("#{FaultType}#", mailModel.FaultType)
                .Replace("#{Priority}#", mailModel.Priority)
                .Replace("#{Description}#", mailModel.Description)
                .Replace("#{StartDate}#", mailModel.StartDate)
                .Replace("#{EndDate}#", mailModel.EndDate)
                .Replace("#{NotifiedBy}#", mailModel.NotifiedBy)
                .Replace("#{AffectedServices}#", mailModel.AffectedServices);
        }

        private string ReplaceArizaBody(string mailText, ArizaModel mailModel)
        {
            mailText = mailText.Replace("#{FaultNo}#", mailModel.FaultNo)
                .Replace("#{FaultType}#", mailModel.FaultType)
                .Replace("#{State}#", mailModel.State)
                .Replace("#{Priority}#", mailModel.Priority)
                .Replace("#{Description}#", mailModel.Description)
                .Replace("#{StartDate}#", mailModel.StartDate)
                .Replace("#{FailureCause}#", mailModel.FailureCause)
                .Replace("#{AlarmStatus}#", mailModel.AlarmStatus ? "Evet" : "Hayır")
                .Replace("#{NotifiedBy}#", mailModel.NotifiedBy)
                .Replace("#{AffectedServices}#", mailModel.AffectedServices)
                .Replace("#{AffectedFirms}#", mailModel.AffectedFirms);

            if (mailModel.State is "Bitti" or "Yaşanmıştır")
                mailText = mailText.Replace("#{EndDate}#", mailModel.EndDate);

            return mailText;
        }
    }
}