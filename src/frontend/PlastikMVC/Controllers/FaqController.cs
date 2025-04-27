using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client;
using PlastikMVC.Client.Models.Response.FaqCategoryResponse;
using PlastikMVC.Client.Models.Response.FaqResponse;
using PlastikMVC.Dtos.CategoryDto;
using PlastikMVC.Dtos.FaqCategoryDto;
using PlastikMVC.Models.Faq;

namespace PlastikMVC.Controllers
{
    public class FaqController : Controller
    {

        private readonly FaqClient _faqClient;
        private readonly FaqCategoryClient _faqCategoryClient;

        public FaqController(FaqClient faqClient, FaqCategoryClient faqCategoryClient)
        {
            _faqClient = faqClient ?? throw new ArgumentNullException(nameof(faqClient));
            _faqCategoryClient = faqCategoryClient ?? throw new ArgumentNullException(nameof(faqCategoryClient));
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            try
            {
                var faqsTask = _faqClient.GetAllFaqsAsync();
                var faqCategoriesTask = _faqCategoryClient.GetAllFaqCategoriesAsync();

                await Task.WhenAll(faqsTask, faqCategoriesTask);

                var faqs = faqsTask.Result ?? new List<GetAllFaqResponse>();
                var faqCategories = faqCategoriesTask.Result ?? new List<GetAllFaqCategoryResponse>();

                // Kategorilere soruları ekle
                foreach (var category in faqCategories)
                {
                    category.Questions = faqs.Where(f => f.CategoryId == category.Id).ToList();
                }

                if (categoryId.HasValue)
                {
                    faqs = faqs.Where(f => f.CategoryId == categoryId.Value).ToList();
                }

                var viewModel = new FaqViewModel
                {
                    Faqs = faqs,
                    FaqCategories = faqCategories,
                    SelectedCategory = faqCategories.FirstOrDefault(c => c.Id == categoryId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                return View("Error");
            }
        }


    }
}
