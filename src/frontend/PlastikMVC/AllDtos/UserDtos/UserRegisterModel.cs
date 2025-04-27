using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos  // DTO CREATED ONR
{
    public class UserRegisterModel :BaseDto
    {
        [Required(ErrorMessage = "Ad alanı doldurulmalıdır")]
        [Display(Name = "Ad")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "İsim alanı doldurulmalıdır")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim alanı doldurulmalıdır")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Şifre alanı doldurulmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre alanı doldurulmalıdır")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "E-Posta alanı doldurulmalıdır")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;

    }
}
