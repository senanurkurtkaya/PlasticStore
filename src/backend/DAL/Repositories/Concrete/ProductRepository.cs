using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository( AppDbContext context)
        {
            _context = context;
        }

     

        public async Task<List<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
               .Include(p => p.Category)
               .Include(p => p.Images.Where(i => i.IsPreviewImage))
               .ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Include(x => x.MetaTags)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
         .Where(p => p.CategoryId == categoryId)
         .Include(p => p.Category) 
         .Include(p => p.Images)   
         .ToListAsync();
        }
    }
}
