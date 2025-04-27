using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ReportsDto
{
    public class RevenueStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalProductsSold { get; set; }
        public List<DailyRevenueDto> DailyRevenue { get; set; }
    }
}
