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
    public class ProductMetaTagsRepository : IProductMetaTagsRepository
    {
        private readonly AppDbContext _dbcontext;
        private readonly DbSet<ProductMetaTags> _dbSet;

        public ProductMetaTagsRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext; 
            _dbSet = _dbcontext.Set<ProductMetaTags>();
        }

        public async Task AddAsync(ProductMetaTags entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbcontext.SaveChangesAsync(); //
        }

        public async Task DeleteAsync(ProductMetaTags entity)
        {
            _dbSet.Remove(entity);
            await _dbcontext.SaveChangesAsync(); //
        }

        public async Task<IEnumerable<ProductMetaTags>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();

        }

        public async Task<ProductMetaTags> GetByIdAsync(int id)
        {

            return await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ProductMetaTags> GetByProductIdAsync(int productId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task UpdateAsync(ProductMetaTags entity)
        {
            _dbSet.Update(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }

        
        
    }
    
}
