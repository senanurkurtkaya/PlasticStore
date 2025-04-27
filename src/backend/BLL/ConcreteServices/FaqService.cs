using BLL.AbstractServices;
using BLL.Dtos.FaqDto;
using DAL.Entities;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class FaqService : IFaqService
    {
        private readonly IGenericRepository<FrequentlyAskedQuestion> _faqRepository;
        private readonly IGenericRepository<FaqCategory> _categoryRepository;
        private readonly ILogger<FaqService> _logger;

        public FaqService(
            IGenericRepository<FrequentlyAskedQuestion> faqRepository,
            IGenericRepository<FaqCategory> categoryRepository,
            ILogger<FaqService> logger)
        {
            _faqRepository = faqRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<FaqDto>> GetAllFaqsAsync()
        {
            _logger.LogInformation("GetAllFaqsAsync() çağrıldı.");
            var faqs = await _faqRepository.GetAll()
                                           .Include(faq => faq.FaqCategory)
                                           .ToListAsync();

            return faqs.Select(faq => new FaqDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                CategoryId = faq.CategoryId,
                CategoryName = faq.FaqCategory?.Name
            }).ToList();
        }


        public async Task<IEnumerable<FaqDto>> GetFaqsByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("GetFaqsByCategoryAsync({CategoryId}) çağrıldı.", categoryId);
            var faqs = await _faqRepository.FindAsync(f => f.CategoryId == categoryId);

            return faqs.Select(faq => new FaqDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                CategoryId = faq.CategoryId,
                CategoryName = faq.FaqCategory?.Name
            }).ToList();
        }


        public async Task<FaqDto> GetFaqByIdAsync(int id)
        {
            _logger.LogInformation("GetFaqByIdAsync({Id}) çağrıldı.", id);
            var faq = await _faqRepository.GetAll()
                                          .Include(f => f.FaqCategory)
                                          .FirstOrDefaultAsync(f => f.Id == id);

            if (faq == null)
            {
                _logger.LogWarning("ID {Id} için herhangi bir SSS bulunamadı.", id);
                return null;
            }

            return new FaqDto
            {
                Id = faq.Id,
                Question = faq.Question,
                Answer = faq.Answer,
                CategoryId = faq.CategoryId,
                CategoryName = faq.FaqCategory?.Name
            };
        }


        public async Task AddFaqAsync(CreateFaqDto faqDto)
        {
            _logger.LogInformation("AddFaqAsync() çağrıldı.");


            var categoryExists = await _categoryRepository.ExistsAsync(faqDto.CategoryId);
            if (!categoryExists)
            {
                _logger.LogWarning("Belirtilen kategori bulunamadı. ID: {CategoryId}", faqDto.CategoryId);
                throw new Exception("Belirtilen kategori bulunamadı.");
            }


            var faq = new FrequentlyAskedQuestion
            {
                Question = faqDto.Question,
                Answer = faqDto.Answer,
                CategoryId = faqDto.CategoryId
            };

            await _faqRepository.AddAsync(faq);
            await _faqRepository.SaveChangesAsync();
        }


        public async Task UpdateFaqAsync(UpdateFaqDto faqDto)
        {
            _logger.LogInformation("UpdateFaqAsync({Id}) çağrıldı.", faqDto.Id);

            var existingFaq = await _faqRepository.GetByIdAsync(faqDto.Id);
            if (existingFaq == null)
            {
                _logger.LogWarning("ID {Id} için herhangi bir SSS bulunamadı.", faqDto.Id);
                throw new Exception("SSS bulunamadı.");
            }

            var categoryExists = await _categoryRepository.ExistsAsync(faqDto.CategoryId);
            if (!categoryExists)
            {
                _logger.LogWarning("Belirtilen kategori bulunamadı. ID: {CategoryId}", faqDto.CategoryId);
                throw new Exception("Belirtilen kategori bulunamadı.");
            }

            existingFaq.Question = faqDto.Question;
            existingFaq.Answer = faqDto.Answer;
            existingFaq.CategoryId = faqDto.CategoryId;

            _faqRepository.Update(existingFaq);
            await _faqRepository.SaveChangesAsync();
        }

        public async Task DeleteFaqAsync(int id)
        {
            _logger.LogInformation("DeleteFaqAsync({Id}) çağrıldı.", id);
            var faq = await _faqRepository.GetByIdAsync(id);
            if (faq != null)
            {
                _faqRepository.Remove(faq);
                await _faqRepository.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("ID {Id} için herhangi bir SSS bulunamadı.", id);
                throw new Exception("SSS bulunamadı.");
            }
        }
    }
}
