using BLL.AbstractServices;
using BLL.Dtos.ProductDto;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IGenericRepository<ProductImage> _genericRepository;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(IProductImageRepository productImageRepository,
                                   IGenericRepository<ProductImage> genericRepository,
                                   ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _genericRepository = genericRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<ProductImageDto>> GetImagesByProductIdAsync(int productId)
        {
            try
            {
                _logger.LogInformation("Fetching images for ProductId: {ProductId}", productId);
                var images = await _productImageRepository.GetByProductIdAsync(productId);

                return images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ProductId = img.ProductId,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    IsPreviewImage = img.IsPreviewImage,


                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving images for ProductId: {ProductId}", productId);
                throw;
            }
        }



        public async Task<bool> DeleteImageAsync(int productId, int imageId)
        {
            try
            {
                var image = await _genericRepository.GetByIdAsync(imageId);
                if (image == null || image.ProductId != productId)
                {
                    _logger.LogWarning("Delete failed: ImageId {ImageId} not found or does not match ProductId {ProductId}", imageId, productId);
                    return false;
                }

                _genericRepository.Remove(image);
                await _genericRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting imageId {ImageId} for ProductId {ProductId}", imageId, productId);
                throw;
            }
        }

        public async Task<ProductImageDto> AddImageAsync(int productId, CreateProductImageDto CreateProductImageDto)
        {
            try
            {
                var addImage = new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = CreateProductImageDto.ImageUrl,
                    AltText = CreateProductImageDto.AltText,
                    IsPreviewImage = CreateProductImageDto.IsPreviewImage,


                };

                await _genericRepository.AddAsync(addImage);
                await _genericRepository.SaveChangesAsync();

                return new ProductImageDto
                {
                    Id = addImage.Id,
                    ProductId = addImage.ProductId,
                    ImageUrl = addImage.ImageUrl,
                    AltText = addImage.AltText,
                    IsPreviewImage = addImage.IsPreviewImage,


                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding image for ProductId: {ProductId}", productId);
                throw;
            }
        }

        public async Task<bool> UpdateImageAsync(int productId, int imageId, UpdateProductImageDto updateProductImageDto)
        {
            try
            {
                var image = await _genericRepository.GetByIdAsync(imageId);

                if (image == null || image.ProductId != productId)
                {
                    _logger.LogWarning("Update failed: ImageId {ImageId} not found or does not match ProductId {ProductId}", imageId, productId);
                    return false;
                }

                image.ImageUrl = updateProductImageDto.ImageUrl;
                image.AltText = updateProductImageDto.AltText;
                image.IsPreviewImage = updateProductImageDto.IsPreviewImage;




                _genericRepository.Update(image);
                await _genericRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating imageId {ImageId} for ProductId {ProductId}", imageId, productId);
                throw;
            }
        }
    }
}
