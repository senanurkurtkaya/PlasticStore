using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Concrete
{
    public class ProductImageRepository : GenericRepository<ProductImage> , IProductImageRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<ProductImage> _dbSet;

        public ProductImageRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet =_dbContext.Set<ProductImage>();
        }
        public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
        {
           return await _dbSet.Where(x => x.ProductId == productId).ToListAsync();
        }
    }
}
