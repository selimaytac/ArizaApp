using System.Collections;
using System.Collections.Generic;

namespace ArizaApp.Models.Entities
{
    public class EmailRecord
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        
        public string EmailDescription { get; set; }
        public IEnumerable<FirmRecord> Firms { get; set; }
    }
}