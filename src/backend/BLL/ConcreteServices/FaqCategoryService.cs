using BLL.AbstractServices;
using BLL.Dtos.CategoryDto;
using BLL.Dtos.FaqCategoryDto;
using BLL.Dtos.FaqDto;
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
    public class FaqCategoryService : IFaqCategoryService
    {

        private readonly IGenericRepository<FaqCategory> _categoryRepository;
        private readonly IGenericRepository<FrequentlyAskedQuestion> _faqRepository;
        private readonly ILogger<FaqCategoryService> _logger;

        public FaqCategoryService(
            IGenericRepository<FaqCategory> categoryRepository,
            IGenericRepository<FrequentlyAskedQuestion> faqRepository,
            ILogger<FaqCategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _faqRepository = faqRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<FaqCategoryDto>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("GetAllCategoriesAsync() çağrıldı.");
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(category => new FaqCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
        }

        public async Task<FaqCategoryWithQuestionsDto> GetCategoryWithFaqsAsync(int categoryId)
        {
            _logger.LogInformation("GetCategoryWithFaqsAsync({CategoryId}) çağrıldı.", categoryId);

            var category = await _categoryRepository.GetAll()
                                                   .Include(c => c.Questions)
                                                   .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                _logger.LogWarning("ID {CategoryId} için herhangi bir kategori bulunamadı.", categoryId);
                return null;
            }

            return new FaqCategoryWithQuestionsDto
            {
                Id = category.Id,
                Name = category.Name,
                Questions = category.Questions.Select(faq => new FaqCategoryDto
                {
                    Id = faq.Id,
                    Question = faq.Question,
                    Answer = faq.Answer
                }).ToList()
            };
        }

        public async Task AddCategoryAsync(CreateCategoryFaqDto categoryDto)
        {
            _logger.LogInformation("AddCategoryAsync() çağrıldı.");

            var category = new FaqCategory
            {
                Name = categoryDto.Name,

            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(UpdateCategoryFaqDto categoryDto)
        {
            _logger.LogInformation("UpdateCategoryAsync({Id}) çağrıldı.", categoryDto.Id);

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (existingCategory == null)
            {
                _logger.LogWarning("ID {Id} için herhangi bir kategori bulunamadı.", categoryDto.Id);
                throw new Exception("Kategori bulunamadı.");
            }

            existingCategory.Name = categoryDto.Name;

            _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("DeleteCategoryAsync({Id}) çağrıldı.", id);
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                _categoryRepository.Remove(category);
                await _categoryRepository.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("ID {Id} için herhangi bir kategori bulunamadı.", id);
                throw new Exception("Kategori bulunamadı.");
            }
        }
    }
}
