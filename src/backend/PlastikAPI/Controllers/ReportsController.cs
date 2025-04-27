using BLL.AbstractServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlastikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
           _reportService = reportService;
        }
       [HttpGet("ProductSales")]
       public async Task<IActionResult> GetProductSales()
        {
            var productSales = await _reportService.GetProductSalesAsync();
            return Ok(productSales);
        }
        [HttpGet("CategorySales")]
        public async Task<IActionResult> GetCategorySales()
        {
            var categorySales = await _reportService.GetCategorySalesAsync();
            return Ok(categorySales);
        }
        [HttpGet("GetRevenueStats")]

        public async Task<IActionResult> GetRevenueStats()
        {
            var getAvenueStats = await _reportService.GetRevenueStatsAsync();
            return Ok(getAvenueStats);
        }
        [HttpGet("SalesTopFive")]
        public async Task<IActionResult> SalesTopFive()
        {
            var salesTopFive = await _reportService.SalesTopFive();
            return Ok(salesTopFive);
        }
    }
}
