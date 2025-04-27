namespace PlastikMVC.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
        public UserResponse User { get; set; }
    }
}
