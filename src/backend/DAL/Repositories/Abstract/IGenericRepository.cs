using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstract
{
    public interface IGenericRepository<TEntity> 
        where TEntity : BaseClass
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(int id, TEntity entity);
        Task DeleteAsync(int id);
        void RemoveRange(IEnumerable<TEntity> entities);
        void SaveChanges();
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(int? categoryId);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IQueryable<TEntity> GetAll();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
