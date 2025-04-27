using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DAL.Repositories.Abstract
{
    public interface IProductMetaTagsRepository 
    {
        Task<IEnumerable<ProductMetaTags>> GetAllAsync();
        Task<ProductMetaTags> GetByIdAsync(int id);
        Task<ProductMetaTags> GetByProductIdAsync(int productId);
        Task AddAsync(ProductMetaTags entity);
        Task UpdateAsync(ProductMetaTags entity);
        Task DeleteAsync(ProductMetaTags entity);
        Task SaveChangesAsync();
    }
}
