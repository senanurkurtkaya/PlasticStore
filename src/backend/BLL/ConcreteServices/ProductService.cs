using BLL.AbstractServices;
using BLL.Dtos.ProductDto;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productGenericRepository;
        private readonly IProductRepository _productRepository;



        public ProductService(IGenericRepository<Product> productGenericRepository, IProductRepository productRepository)
        {
            _productGenericRepository = productGenericRepository;
            _productRepository = productRepository;
        }
        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {

            var product = new Product

            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                Images = createProductDto.ProductImages.Select(X => new ProductImage
                {
                    ImageUrl = X.ImageUrl,
                    AltText = X.AltText,
                    IsPreviewImage = X.IsPreviewImage

                }).ToList(),
                CategoryId = createProductDto.CategoryId,
                IsActive = createProductDto.IsActive,
                StockQuantity = createProductDto.StockQuantity,

            };

            await _productGenericRepository.AddAsync(product);

            await _productGenericRepository.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productGenericRepository.GetByIdAsync(id);
            if (product == null) return false;

            _productGenericRepository.Remove(product);
            await _productGenericRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int? categoryId)
        {
            if (categoryId == null || categoryId <= 0)
            {
                return false; // Geçersiz ID durumlarını kontrol et
            }

            // Veri tabanında kategori var mı kontrol et
            return await _productGenericRepository.ExistsAsync(categoryId);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllWithCategoryAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive,
                CategoryName = p.Category.Name,
                PreviewImageUrl = p.Images.FirstOrDefault(i => i.IsPreviewImage)?.ImageUrl
            }).ToList();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            //var product = await _productGenericRepository.GetByIdAsync(id);
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryName = product.Category.Name,
                Images = product.Images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                }).ToList(),
                MetaTags = product.MetaTags != null ? new ProductMetaTagsDto
                {
                    Id = product.MetaTags.Id,
                    Description = product.MetaTags.Description,
                    Title = product.MetaTags.Title,
                    OpenGraphDescription = product.MetaTags.OpenGraphDescription,
                    OpenGraphImage = product.MetaTags.OpenGraphImage,
                    OpenGraphTitle = product.MetaTags.OpenGraphTitle,
                    OpenGraphType = product.MetaTags.OpenGraphType,
                    OpenGraphUrl = product.MetaTags.OpenGraphUrl,
                    TwitterCard = product.MetaTags.TwitterCard,
                    TwitterDescription = product.MetaTags.TwitterDescription,
                    TwitterImage = product.MetaTags.TwitterImage,
                    TwitterTitle = product.MetaTags.TwitterTitle,
                    TwitterUrl = product.MetaTags.TwitterUrl,
                } : null,
                PreviewImageUrl = product.Images.FirstOrDefault(i => i.IsPreviewImage)?.ImageUrl,
            };
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(int id)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(id);

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsActive = p.IsActive,
                PreviewImageUrl = p.Images.FirstOrDefault(i => i.IsPreviewImage)?.ImageUrl,
                StockQuantity = p.StockQuantity



            }).ToList();

            return productDtos;
        }

        public async Task<List<ProductDto>> GetTop20Async()
        {
            return await _productGenericRepository.GetAll()
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryName = x.Category.Name,
                    Description = x.Description,
                    Price = x.Price,
                    PreviewImageUrl = x.Images.FirstOrDefault(i => i.IsPreviewImage).ImageUrl,
                })
                .Take(20)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _productGenericRepository.GetByIdAsync(id);
            if (product == null) return false;


            if (updateProductDto.StockQuantity < 0)
                throw new ArgumentException("Stock quantity cannot be negative.");

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.Description = updateProductDto.Description;
            product.CategoryId = updateProductDto.CategoryId;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.IsActive = updateProductDto.IsActive;


            _productGenericRepository.Update(product);
            await _productGenericRepository.SaveChangesAsync();
            return true;
        }
    }
}

