using BLL.AbstractServices;
using BLL.Dtos.FaqDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlastikAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaqController : Controller
    {
        private readonly IFaqService _faqService;
        private readonly ILogger<FaqController> _logger;

        public FaqController(IFaqService faqService, ILogger<FaqController> logger)
        {
            _faqService = faqService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFaqs()
        {
            _logger.LogInformation("GetAllFaqs() çağrıldı.");
            var faqs = await _faqService.GetAllFaqsAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFaqById(int id)
        {
            _logger.LogInformation("GetFaqById({Id}) çağrıldı.", id);
            var faq = await _faqService.GetFaqByIdAsync(id);
            if (faq == null)
            {
                return NotFound($"SSS ID {id} bulunamadı.");
            }
            return Ok(faq);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> AddFaq([FromBody] CreateFaqDto faqDto)
        {
            _logger.LogInformation("AddFaq() çağrıldı.");
            await _faqService.AddFaqAsync(faqDto);
            return CreatedAtAction(nameof(GetFaqById), new { id = faqDto.CategoryId }, faqDto);
        }

     
        [HttpGet("category/{categoryId}")]
        [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> GetFaqsByCategory(int categoryId)
        {
            _logger.LogInformation("GetFaqsByCategory({CategoryId}) çağrıldı.", categoryId);
            var faqs = await _faqService.GetFaqsByCategoryAsync(categoryId);
            return Ok(faqs);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> UpdateFaq(int id, [FromBody] UpdateFaqDto faqDto)
        {
            faqDto.Id = id;
            _logger.LogInformation("UpdateFaq({Id}) çağrıldı.", faqDto.Id);
            await _faqService.UpdateFaqAsync(faqDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            _logger.LogInformation("DeleteFaq({Id}) çağrıldı.", id);
            await _faqService.DeleteFaqAsync(id);
            return NoContent();
        }
    }
}
