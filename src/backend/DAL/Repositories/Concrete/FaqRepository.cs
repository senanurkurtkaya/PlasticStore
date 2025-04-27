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
    public class FaqRepository : GenericRepository<FrequentlyAskedQuestion>, IFaqRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<FrequentlyAskedQuestion> _dbSet;

        public FaqRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<FrequentlyAskedQuestion>();
        }

        public async Task<IEnumerable<FrequentlyAskedQuestion>> GetFaqsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(f => f.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}

