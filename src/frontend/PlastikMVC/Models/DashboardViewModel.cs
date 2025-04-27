using PlastikMVC.AllDtos.UserDtos;

namespace PlastikMVC.Models
{
    public class DashboardViewModel
    {

        public IEnumerable<UserDto> GetAllUsers { get; set; }
        public int DailyActiveUsers { get; set; }
    }
}
