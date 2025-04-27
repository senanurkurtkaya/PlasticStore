using BLL.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IProductMetaTagsService
    {
        Task<IEnumerable<ProductMetaTagsDto>> GetAllAsync(); // Tüm MetaTag'leri getir
        Task<ProductMetaTagsDto> GetByIdAsync(int id); // ID'ye göre getir
        Task<ProductMetaTagsDto> GetByProductIdAsync(int productId); // ProductId'ye göre getir
        Task CreateAsync(CreateProductMetaTagsDto createDto); // Yeni MetaTag ekle
        Task UpdateAsync(int id, UpdateProductMetaTagsDto updateDto); // Güncelleme yap
        Task DeleteAsync(int id); // MetaTag sil
    }
}
