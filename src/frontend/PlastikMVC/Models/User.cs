using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlastikMVC.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "İsim alanı doldurulmalıdır.")]
        [Display(Name = "İsim")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim alanı doldurulmalıdır.")]
        [Display(Name = "Soyisim")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı doldurulmalıdır.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı doldurulmalıdır.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [NotMapped] // Veritabanında saklanmayacak
        [Required(ErrorMessage = "Şifre alanı doldurulmalıdır.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords does not match!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "E-posta alanı doldurulmalıdır.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }
    }
}
