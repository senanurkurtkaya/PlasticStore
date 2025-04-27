using BLL.AbstractServices;
using BLL.Dtos.ProductDto;
using DAL.Entities;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class ProductMetaTagsService : IProductMetaTagsService
    {
        private readonly IProductMetaTagsRepository _repository;
        private readonly IGenericRepository<Product> _productGenericRepository;

        public ProductMetaTagsService(IProductMetaTagsRepository repository, IGenericRepository<Product> productGenericRepository)
        {
            _repository = repository;
            _productGenericRepository = productGenericRepository;
        }

        public async Task<IEnumerable<ProductMetaTagsDto>> GetAllAsync()
        {  //buna gerek yok!!
            var entities = await _repository.GetAllAsync();
            return entities.Select(x => new ProductMetaTagsDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                OpenGraphType = x.OpenGraphType,
                OpenGraphUrl = x.OpenGraphUrl,
                OpenGraphTitle = x.OpenGraphTitle,
                OpenGraphDescription = x.OpenGraphDescription,
                OpenGraphImage = x.OpenGraphImage,
                TwitterCard = x.TwitterCard,
                TwitterUrl = x.TwitterUrl,
                TwitterTitle = x.TwitterTitle,
                TwitterDescription = x.TwitterDescription,
                TwitterImage = x.TwitterImage,
                ProductId = x.ProductId
            }).ToList();
        }

        public async Task<ProductMetaTagsDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByProductIdAsync(id);
            if (entity == null) return null;

            return new ProductMetaTagsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                OpenGraphType = entity.OpenGraphType,
                OpenGraphUrl = entity.OpenGraphUrl,
                OpenGraphTitle = entity.OpenGraphTitle,
                OpenGraphDescription = entity.OpenGraphDescription,
                OpenGraphImage = entity.OpenGraphImage,
                TwitterCard = entity.TwitterCard,
                TwitterUrl = entity.TwitterUrl,
                TwitterTitle = entity.TwitterTitle,
                TwitterDescription = entity.TwitterDescription,
                TwitterImage = entity.TwitterImage,
                ProductId = entity.ProductId
            };
        }

        public async Task<ProductMetaTagsDto> GetByProductIdAsync(int productId)
        {
            var entity = await _repository.GetByProductIdAsync(productId);
            if (entity == null) return null;

            return new ProductMetaTagsDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                OpenGraphType = entity.OpenGraphType,
                OpenGraphUrl = entity.OpenGraphUrl,
                OpenGraphTitle = entity.OpenGraphTitle,
                OpenGraphDescription = entity.OpenGraphDescription,
                OpenGraphImage = entity.OpenGraphImage,
                TwitterCard = entity.TwitterCard,
                TwitterUrl = entity.TwitterUrl,
                TwitterTitle = entity.TwitterTitle,
                TwitterDescription = entity.TwitterDescription,
                TwitterImage = entity.TwitterImage,
                ProductId = entity.ProductId
            };
        }

        public async Task CreateAsync(CreateProductMetaTagsDto createDto)
        {
            var product = await _productGenericRepository.GetByIdAsync(createDto.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException($"Product not found with Id: {createDto.ProductId}");
            }

            if (product.MetaTagsId.HasValue)
            {
                throw new InvalidOperationException($"Product with Id: {product.Id} already has MetaTags. Use PUT endpoint to update MetaTags.");
            }

            var entity = new ProductMetaTags
            {
                Title = createDto.Title,
                Description = createDto.Description,
                OpenGraphType = createDto.OpenGraphType,
                OpenGraphUrl = createDto.OpenGraphUrl,
                OpenGraphTitle = createDto.OpenGraphTitle,
                OpenGraphDescription = createDto.OpenGraphDescription,
                OpenGraphImage = createDto.OpenGraphImage,
                TwitterCard = createDto.TwitterCard,
                TwitterUrl = createDto.TwitterUrl,
                TwitterTitle = createDto.TwitterTitle,
                TwitterDescription = createDto.TwitterDescription,
                TwitterImage = createDto.TwitterImage,
                ProductId = createDto.ProductId


            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            product.MetaTagsId = entity.Id;
            _productGenericRepository.Update(product);

            await _productGenericRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateProductMetaTagsDto updateDto)
        {
            var entity = await _repository.GetByProductIdAsync(id);
            if (entity == null) return;

            entity.Title = updateDto.Title;
            entity.Description = updateDto.Description;
            entity.OpenGraphType = updateDto.OpenGraphType;
            entity.OpenGraphUrl = updateDto.OpenGraphUrl;
            entity.OpenGraphTitle = updateDto.OpenGraphTitle;
            entity.OpenGraphDescription = updateDto.OpenGraphDescription;
            entity.OpenGraphImage = updateDto.OpenGraphImage;
            entity.TwitterCard = updateDto.TwitterCard;
            entity.TwitterUrl = updateDto.TwitterUrl;
            entity.TwitterTitle = updateDto.TwitterTitle;
            entity.TwitterDescription = updateDto.TwitterDescription;
            entity.TwitterImage = updateDto.TwitterImage;

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();



        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;

            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
        }
    }
}

