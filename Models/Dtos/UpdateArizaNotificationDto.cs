using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class UpdateArizaNotificationDto
    {
        public int Id { get; set; }

        [DisplayName("Bülten No")] [Required] public string FaultNo { get; set; }

        [DisplayName("Problem İle İlgilenen Ekip")]
        [Required]
        public string NotifiedBy { get; set; }

        [DisplayName("Mail Konusu")]
        [Required]
        public string MailSubject { get; set; }

        [DisplayName("Bülten Tipi")] public string FaultType { get; set; }
        [Required] [DisplayName("Durum")] public string State { get; set; }

        [Required] [DisplayName("Öncelik")] public string Priority { get; set; }

        [Required] [DisplayName("Açıklama")] public string Description { get; set; }

        [DisplayName("Başlangıç Zamanı")] public string StartDate { get; set; }

        [DisplayName("Bitiş Zamanı")] public string EndDate { get; set; }

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
        
        [DisplayName("Kesintiyi Onaylayan Yönetici")]
        public string ApprovedBy { get; set; }

        [Required]
        [DisplayName("Tekrar Mail Gönderimi")]
        public bool SendMailAgain { get; set; } = false;
    }
}