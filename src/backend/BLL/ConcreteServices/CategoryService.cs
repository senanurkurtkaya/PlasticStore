using BLL.AbstractServices;
using BLL.Dtos.CategoryDto;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryGenericRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryGenericRepository, ICategoryRepository categoryRepository)
        {
            _categoryGenericRepository = categoryGenericRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
            };

            await _categoryGenericRepository.AddAsync(category);
            await _categoryGenericRepository.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                

            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryGenericRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryGenericRepository.DeleteAsync(id);
            await _categoryGenericRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int? categoryId)
        {
            return await _categoryGenericRepository.ExistsAsync(categoryId);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllWithProductsAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StockQuantity = c.Products.Sum(x => x.StockQuantity)
            }).ToList();
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithProductsAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                StockQuantity = category.Products.Sum(x => x.StockQuantity)
            };
        }

        //public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        //{
        //    // Aynı isimde kategori var mı kontrol et
        //    if (await ExistsByNameAsync(createCategoryDto.Name))
        //    {
        //        throw new Exception("Bu isimde bir kategori zaten mevcut.");
        //    }

        //    var category = new Category
        //    {
        //        Name = createCategoryDto.Name,
        //        Description = createCategoryDto.Description
        //    };

        //    await _categoryGenericRepository.AddAsync(category);
        //    await _categoryGenericRepository.SaveChangesAsync();

        //    return new CategoryDto
        //    {
        //        Id = category.Id,
        //        Name = category.Name,
        //        Description = category.Description
        //    };
        //}

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryGenericRepository.GetByIdAsync(id);
            if (category == null) 
                return false;

            // Aynı isimde başka bir kategori var mı kontrol et
            var existingCategory = await _categoryGenericRepository.AnyAsync(c => c.Name == updateCategoryDto.Name && c.Id != id);
            if (existingCategory)
            {
                throw new Exception("Bu isimde başka bir kategori zaten mevcut.");
            }

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;

            await _categoryGenericRepository.UpdateAsync(id,category);
            await _categoryGenericRepository.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var category = await _categoryGenericRepository.GetByIdAsync(id);
        //    if (category == null) return false;

        //    _categoryGenericRepository.Remove(category);
        //    await _categoryGenericRepository.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> ExistsAsync(int? categoryId)
        //{
        //    if (!categoryId.HasValue) return false; // Null ID olamaz
        //    return await _categoryGenericRepository.AnyAsync(c => c.Id == categoryId.Value);
        //}

        public async Task<bool> ExistsByNameAsync(string categoryName)
        {
            return await _categoryGenericRepository.AnyAsync(c => c.Name == categoryName);
        }
    }
}
