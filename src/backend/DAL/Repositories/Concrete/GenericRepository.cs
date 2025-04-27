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

    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseClass
    {

        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext contex)
        {
            _context = contex;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> ExistsAsync(int? id)
        {
            return await _dbSet.AnyAsync(entity => entity.Id == id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var deletedEntity = await _dbSet.FindAsync(id);

            if (deletedEntity == null)
            {
                throw new KeyNotFoundException("Silinecek veri bulunamadı!");
            }
            _dbSet.Remove(deletedEntity);
            await _context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TEntity entity)
        {
            var updatedEntity = await _dbSet.FindAsync(id);

            if (updatedEntity == null)
            {
                throw new KeyNotFoundException("Güncellenecek içerik bulunamadı!");
            }


            _context.Entry(updatedEntity).CurrentValues.SetValues(entity);

            _context.Update(updatedEntity);

            await _context.SaveChangesAsync();

        }

        async Task<List<TEntity>> IGenericRepository<TEntity>.GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task DeleteAsync(Category category)
        {
            throw new NotImplementedException();
        }


        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }


        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }
    }
}
