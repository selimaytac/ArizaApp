using System.Collections;
using System.Collections.Generic;

namespace ArizaApp.Models.Entities
{
    public class FirmRecord
    {
        public int Id { get; set; }
        public string FirmName { get; set; }
        public IEnumerable<EmailRecord> Emails { get; set; } 
        public IEnumerable<ArizaModel> ArizaModels { get; set; } 
    }
}