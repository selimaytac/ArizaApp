using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateFirmDto
    {
        [Required]
        [DisplayName("Firma Adı")]
        public string FirmName { get; set; }
        public int[]  EmailIds { get; set; } 
    }
}