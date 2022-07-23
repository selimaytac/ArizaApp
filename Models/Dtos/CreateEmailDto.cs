using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateEmailDto
    {
        [DisplayName("Email Adresi")]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [DisplayName("Email Adresi Açıklaması")]
        [Required]
        public string EmailDescription { get; set; }
    }
}