using System.ComponentModel.DataAnnotations;

namespace ArizaApp.Models.Dtos
{
    public class ChangePasswordDto
    {
        [Display(Name = "Eski Şifreniz")]
        [Required(ErrorMessage = "Şifre değiştirmek için eski şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        public string PasswordOld { get; set; }    
        
        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Şifre değiştirmek için yeni şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        public string PasswordNew { get; set; }     
        
        [Display(Name = "Yeni Şifreyi Doğrula")]
        [Required(ErrorMessage = "Şifre değiştirmek için yeni şifrenizi doğrulamanız gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakter olmalıdır.")]
        [Compare("PasswordNew", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string PasswordConfirm { get; set; }        
    }
}