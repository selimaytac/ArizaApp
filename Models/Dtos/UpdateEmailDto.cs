using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ArizaApp.Models.Entities;

namespace ArizaApp.Models.Dtos
{
    public class UpdateEmailDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Email Adresi")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        [DisplayName("Email Adresi Açıklaması")]
        public string EmailDescription { get; set; }
    }
}