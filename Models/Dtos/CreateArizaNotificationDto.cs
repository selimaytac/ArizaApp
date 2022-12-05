using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ArizaApp.Models.Dtos
{
    public class CreateArizaNotificationDto
    {
        [DisplayName("Bülten No")] [Required] public string FaultNo { get; set; }

        [DisplayName("Problem ile İlgilenen Ekip")]
        [Required]
        public string NotifiedBy { get; set; }

        [DisplayName("Mail Konusu")] [Required] public string MailSubject { get; set; }

        [DisplayName("Bülten Tipi")] public string FaultType { get; set; }
        [Required] [DisplayName("Durum")] public string State { get; set; }

        [Required] [DisplayName("Öncelik")] public string Priority { get; set; }

        [Required] [DisplayName("Açıklama")] public string Description { get; set; }

        [DisplayName("Başlangıç Zamanı")]
        public string StartDate { get; set; }

        [DisplayName("Bitiş Zamanı")]
        public string EndDate { get; set; }

        [Required]
        [DisplayName("Arıza Sebebi")]
        public string FailureCause { get; set; }

        [Required]
        [DisplayName("Alarm Var mı?")]
        public bool AlarmStatus { get; set; }

        [Required]
        [DisplayName("Etkilenen Servisler")]
        public string AffectedServices { get; set; }

        [Required]
        [DisplayName("Etkilenen Firmalar")]
        public string AffectedFirms { get; set; }

        [Required]
        [DisplayName("Mail Gönderimi")]
        public bool SendMail { get; set; }
            
        [DisplayName("Kesintiyi Onaylayan Yönetici")]
        public string ApprovedBy { get; set; }

        public IEnumerable<int> FirmIdS { get; set; }
        
    }
}