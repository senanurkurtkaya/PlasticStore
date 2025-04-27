using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos
{
    public class UserLoginModel : BaseDto   
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        
    }
}
