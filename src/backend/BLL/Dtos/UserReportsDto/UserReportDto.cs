using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.UserReportsDto
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
