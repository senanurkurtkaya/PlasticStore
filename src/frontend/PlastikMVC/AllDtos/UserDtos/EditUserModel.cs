using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.UserDtos  // DTO CREATED ONR
{
    public class EditUserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
