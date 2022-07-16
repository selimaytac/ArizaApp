using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class UpdateFirmDto
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Firma Adı")]
        public string FirmName { get; set; }
        public int[]  EmailIds { get; set; } 
    }
}