#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ArizaApp.Models.Entities
{
    public class AppUser : IdentityUser
    {
        [Required] public string Name { get; set; }

        [Required] public string Surname { get; set; }

        [Required] public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int SendCount { get; set; } = 0;
        public string? Note { get; set; }
    }
}