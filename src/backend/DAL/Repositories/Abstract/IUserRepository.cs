using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<List<User>> GetAllAsync();
        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate);
        Task AddAsync(User entity);
        Task AddRangeAsync(IEnumerable<User> entities);
        Task UpdateAsync(string id, User entity);
        Task DeleteAsync(string id);
        void RemoveRange(IEnumerable<User> entities);
        void SaveChanges();
        Task SaveChangesAsync();
        Task<bool> ExistsAsync(string? userId);
        void Update(User entity);
        void Remove(User entity);
        IQueryable<User> GetAll();
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);
    }
}
