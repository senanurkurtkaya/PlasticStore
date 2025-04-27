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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task AddAsync(User entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<User> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> ExistsAsync(string? id)
        {
            return await _dbSet.AnyAsync(entity => entity.Id == id);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task DeleteAsync(string id) 
        {
            var deletedEntity = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (deletedEntity == null)
            {
                throw new KeyNotFoundException("Silinecek veri bulunamadı!");
            }
            _dbSet.Remove(deletedEntity);
            await _context.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<User> entities)
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

        public async Task UpdateAsync(string id, User entity)  
        {
            var updatedEntity = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (updatedEntity == null)
            {
                throw new KeyNotFoundException("Güncellenecek içerik bulunamadı!");
            }

            _context.Entry(updatedEntity).CurrentValues.SetValues(entity);
            _context.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task DeleteAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(User entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<User> GetAll()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>().AnyAsync(predicate);
        }

    }

}
