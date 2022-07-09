using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateFirmDto
    {
        public int Id { get; set; }
        [Required]
        public string FirmName { get; set; }
        public int[]  EmailIds { get; set; } 
    }
}