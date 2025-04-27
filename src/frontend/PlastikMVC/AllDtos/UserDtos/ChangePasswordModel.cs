using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Mevcut şifre gereklidir.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre tekrarı gereklidir.")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
