using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ArizaApp.Models.Entities
{
    public class ArizaModel
    {
        public int Id { get; set; }

        [DisplayName("Bülten No")] public string FaultNo { get; set; }

        [DisplayName("Problem İle İlgilenen Ekip")]
        public string NotifiedBy { get; set; }
        [DisplayName("Mail Konusu")] public string MailSubject {get; set;}
        [DisplayName("Bülten Tipi")] public string FaultType { get; set; }
        [DisplayName("Durum")] public string State { get; set; }
        [DisplayName("Öncelik")] public string Priority { get; set; }
        [DisplayName("Açıklama")] public string Description { get; set; }
        [DisplayName("Başlangıç Zamanı")] public string StartDate { get; set; }
        [DisplayName("Bitiş Zamanı")] public string EndDate { get; set; }
        [DisplayName("Bülten Sebebi")] public string FailureCause { get; set; }
        [DisplayName("Alarm Durumu")] public bool AlarmStatus { get; set; }
        [DisplayName("Etkilenen Servisler")] public string AffectedServices { get; set; }
        [DisplayName("Etkilenen Firmalar")] public string AffectedFirms { get; set; }
        [DisplayName("Mail Gönderimi")] public bool SendMail { get; set; }
        [DisplayName("Kesintiyi Onaylayan Yönetici")]
        public string ApprovedBy { get; set; }
        public string UserId { get; set; }
        
        [DisplayName("Bülten Kayıt Tarihi")] 
        public DateTime CreatedDate { get; set; }
        public AppUser User { get; set; }
        public IEnumerable<FirmRecord> Firms { get; set; }
        public IEnumerable<UploadedFileRecords> UploadedFileRecords { get; set; }
    }
}