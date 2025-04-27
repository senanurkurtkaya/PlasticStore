namespace PlastikMVC.AllDtos.UserDtos
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; } // Kullanıcının profil resmi URL'si
        public DateTime RegistrationDate { get; set; } = DateTime.Now; // Kullanıcının kayıt tarihi
        
    }
}
