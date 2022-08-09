using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateArizaNotificationDto
    {
        [DisplayName("Bülten No")] [Required] public string FaultNo { get; set; }

        [DisplayName("Kesinti Bildirimi Yapan Kişi")]
        [Required]
        public string NotifiedBy { get; set; }

        [DisplayName("Mail Konusu")] [Required] public string MailSubject { get; set; }

        [DisplayName("Bülten Tipi")] public string FaultType { get; set; }
        [Required] [DisplayName("Durum")] public string State { get; set; }

        [Required] [DisplayName("Öncelik")] public string Priority { get; set; }

        [Required] [DisplayName("Açıklama")] public string Description { get; set; }

        [Required]
        [DisplayName("Başlangıç Zamanı")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayName("Bitiş Zamanı")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Arıza Sebebi")]
        public string FailureCause { get; set; }

        [Required]
        [DisplayName("Alarm Durumu")]
        public bool AlarmStatus { get; set; }

        [Required]
        [DisplayName("Etiklenen Servisler")]
        public string AffectedServices { get; set; }

        [Required]
        [DisplayName("Etiklenen Firmalar")]
        public string AffectedFirms { get; set; }

        [Required]
        [DisplayName("Mail Gönderimi")]
        public bool SendMail { get; set; }

        [Required]
        [DisplayName("Kesintiyi Onaylayan Yönetici")]
        public string ApprovedBy { get; set; }
        
        public IEnumerable<int> FirmIdS { get; set; }
    }
}