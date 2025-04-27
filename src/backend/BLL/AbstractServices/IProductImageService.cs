using BLL.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IProductImageService
    {
      
        Task<IEnumerable<ProductImageDto>> GetImagesByProductIdAsync(int productId);
        Task<ProductImageDto> AddImageAsync(int productId, CreateProductImageDto CreateProductImageDto);
        Task<bool> UpdateImageAsync(int productId, int imageId, UpdateProductImageDto updateProductImageDto);
        Task<bool> DeleteImageAsync(int productId, int imageId);
    }
}
