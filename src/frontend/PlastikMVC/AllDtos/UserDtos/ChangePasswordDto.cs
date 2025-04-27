using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos
{
    public class ChangePasswordDto
    {
        [Required]
        public string Id { get; set; } // Kullanıcı ID'si

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmNewPassword { get; set; }
    }
}
