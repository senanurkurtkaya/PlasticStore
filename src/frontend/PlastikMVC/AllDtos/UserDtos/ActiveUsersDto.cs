namespace PlastikMVC.AllDtos.UserDtos
{
    public class ActiveUsersDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
