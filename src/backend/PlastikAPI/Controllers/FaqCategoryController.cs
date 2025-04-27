using BLL.AbstractServices;
using BLL.Dtos.CategoryDto;
using BLL.Dtos.FaqCategoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlastikAPI.Controllers
{
    namespace PlastikAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class FaqCategoryController : Controller
        {
            private readonly IFaqCategoryService _faqCategoryService;
            private readonly ILogger<FaqCategoryController> _logger;

            public FaqCategoryController(IFaqCategoryService faqCategoryService, ILogger<FaqCategoryController> logger)
            {
                _faqCategoryService = faqCategoryService;
                _logger = logger;
            }

            [HttpGet]
            public async Task<IActionResult> GetAllCategories()
            {
                _logger.LogInformation("GetAllCategories() çağrıldı.");
                var categories = await _faqCategoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }

            [HttpGet("{categoryId}/faqs")]
            public async Task<IActionResult> GetCategoryWithFaqs(int categoryId)
            {
                _logger.LogInformation("GetCategoryWithFaqs({CategoryId}) çağrıldı.", categoryId);
                var category = await _faqCategoryService.GetCategoryWithFaqsAsync(categoryId);
                if (category == null)
                {
                    return NotFound($"Kategori ID {categoryId} bulunamadı.");
                }
                return Ok(category);
            }

            [HttpPost]
            [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
            public async Task<IActionResult> AddCategory([FromBody] CreateCategoryFaqDto categoryDto)
            {
                _logger.LogInformation("AddCategory() çağrıldı.");
                await _faqCategoryService.AddCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetAllCategories), new { id = categoryDto.Name }, categoryDto);
            }

            [HttpPut("{id}")]
            [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
            public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryFaqDto categoryDto)
            {
                categoryDto.Id = id;
                _logger.LogInformation("UpdateCategory({Id}) çağrıldı.", categoryDto.Id);
                await _faqCategoryService.UpdateCategoryAsync(categoryDto);
                return NoContent();
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
            public async Task<IActionResult> DeleteCategory(int id)
            {
                _logger.LogInformation("DeleteCategory({Id}) çağrıldı.", id);
                await _faqCategoryService.DeleteCategoryAsync(id);
                return NoContent();
            }

        }
    }
}
