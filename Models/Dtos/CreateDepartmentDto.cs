using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateDepartmentDto
    {
        [DisplayName("Departman Adı")]
        [Required]
        public string DepartmentName { get; set; }

        [DisplayName("Departman Açıklaması")]
        [Required]
        public string Description { get; set; }
    }
}