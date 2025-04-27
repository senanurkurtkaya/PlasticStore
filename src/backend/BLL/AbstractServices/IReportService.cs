using BLL.Dtos.ReportsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IReportService
    {
        Task<List<ProductSalesDto>> GetProductSalesAsync();
        Task<List<CategorySalesDto>> GetCategorySalesAsync();
        Task<RevenueStatsDto> GetRevenueStatsAsync();
        Task<List<ProductSalesDto>> SalesTopFive();
    }
}
