using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifreyi Doğrula")]
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmalıdır.")]
        [Compare("Password")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [EmailAddress]
        [DisplayName("Email Adresi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "İsim gereklidir.")]
        [DisplayName("İsim")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim gereklidir.")]
        [DisplayName("Soyisim")]
        public string Surname { get; set; }

        [Required] public string DepartmentId { get; set; }

        [DisplayName("Eklenecek Not (Boş bırakılabilir)")]
        public string Note { get; set; }

        [Required] public string RoleId { get; set; }
    }
}