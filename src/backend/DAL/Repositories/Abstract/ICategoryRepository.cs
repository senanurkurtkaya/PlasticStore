using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstract
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllWithProductsAsync();

        Task<Category> GetByIdWithProductsAsync(int id);
    }
}
