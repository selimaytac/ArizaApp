using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class LoginDto
    {
        [Display(Name = "Email Adresiniz")] 
        [Required(ErrorMessage = "Email alanı gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmalıdır.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}