namespace PlastikMVC.AllDtos.UserDtos
{
    public class UserReportDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int UsersCreatedThisMonth { get; set; }
        public int AdminUsers { get; set; }
    }
}
