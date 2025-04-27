using DAL.Entities;
using DAL.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstract
{
    public interface IProductRepository 
    {

    
        Task<List<Product>> GetAllWithCategoryAsync();

    
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);

        Task<Product> GetProductById(int id);


    }
}
