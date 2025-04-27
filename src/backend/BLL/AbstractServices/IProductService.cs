using BLL.Dtos.ProductDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<List<ProductDto>> GetProductsByCategoryAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto);
      
        Task<List<ProductDto>> GetTop20Async();



    }
}
