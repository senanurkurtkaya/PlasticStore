using BLL.AbstractServices;
using BLL.Dtos.ReportsDto;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<ProductSalesDto>> GetProductSalesAsync()
        {
            return await _db.Products.Select(p => new ProductSalesDto
            {
                ProductName = p.Name,
                SalesCount = p.QuantitySold,
                TotalRevenue = p.QuantitySold * p.Price,
            }).ToListAsync();
        }
        public async Task<RevenueStatsDto> GetRevenueStatsAsync()
        {
            var totalProductsSold = await _db.Products.SumAsync(p => p.QuantitySold);
            var totalRevenue = await _db.Products.SumAsync(p => p.QuantitySold * p.Price);

            var dailyRevenue = await _db.Products.GroupBy(p => p.CreatedDate.Date).Select(g => new DailyRevenueDto
            {
               Date = g.Key,
               Revenue = g.Sum(p=>p.QuantitySold*p.Price)
            }).ToListAsync();

            return new RevenueStatsDto
            {
                TotalRevenue = totalRevenue,
                TotalProductsSold = totalProductsSold,
                DailyRevenue = dailyRevenue
            };

        }

        public async Task<List<ProductSalesDto>> SalesTopFive()
        {

            var topFiveSales = await _db.Products.OrderByDescending(p=>p.QuantitySold).Take(5).Select(p=> new ProductSalesDto
            {
                ProductName= p.Name,
                SalesCount = p.QuantitySold,
                TotalRevenue = p.QuantitySold*p.Price,
            }).ToListAsync();

            return topFiveSales;

        }

        async Task<List<CategorySalesDto>> IReportService.GetCategorySalesAsync()
        {
            return await _db.Categories.Select(c => new CategorySalesDto
            {
              CategoryName = c.Name,
              SalesCount = c.Products.Sum(p=>p.QuantitySold),
              TotalRevenue = c.Products.Sum(p=>p.QuantitySold*p.Price)
            }).ToListAsync();
        }
    }
}
