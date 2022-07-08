#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArizaApp.Models.Entities
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
        
        [Required]
        [Display(Name = "Bağlı Olduğu Departman")]
        public string Department { get; set; }

        public string? Note { get; set; }
    }
}