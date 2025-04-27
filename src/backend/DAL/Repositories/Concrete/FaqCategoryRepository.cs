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
    public class FaqCategoryRepository : GenericRepository<FaqCategory>, IFaqCategoryRepository
    {

        private readonly AppDbContext _dbContext;
        private readonly DbSet<FaqCategory> _dbSet;

        public FaqCategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<FaqCategory>();
        }

        public async Task<IEnumerable<FaqCategory>> GetCategoriesWithFaqsAsync()
        {
            return await _dbSet
                .Include(fc => fc.Questions)
                .ToListAsync();
        }
    }
}
