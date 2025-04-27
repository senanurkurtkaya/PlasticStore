using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.UserReportsDto
{
    public class UpdateLastLoginDto
    {

        public string UserId { get; set; }
        public DateTime LastLoginDate { get; set; }

    }
}
