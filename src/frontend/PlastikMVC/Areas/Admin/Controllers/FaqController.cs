using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client.Models.Request.FaqRequest;
using PlastikMVC.Client.Models.Response.FaqResponse;
using PlastikMVC.Client;
using Microsoft.AspNetCore.Authorization;
using PlastikMVC.Filters;

namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqController : Controller
    {
        private readonly FaqClient _faqClient;
        private readonly FaqCategoryClient _faqCategoryClient;
        private readonly ILogger<FaqController> _logger;

        public FaqController(FaqClient faqClient, ILogger<FaqController> logger, FaqCategoryClient faqCategoryClient)
        {
            _faqClient = faqClient;
            _logger = logger;
            _faqCategoryClient = faqCategoryClient;
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var faqs = await _faqClient.GetAllFaqsAsync();
                return View(faqs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSS listesi alınırken bir hata oluştu.");
                TempData["ErrorMessage"] = "SSS'ler yüklenirken bir hata oluştu.";
                return View(new List<GetAllFaqResponse>());
            }
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCreate()
        {
            var faqCategories = await _faqCategoryClient.GetAllFaqCategoriesAsync();

            ViewBag.FaqCategories = faqCategories;

            return View();
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCreate(CreateFaqRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Admin/Views/Faq/FaqCreate.cshtml", request);
            }

            try
            {
                await _faqClient.CreateFaqAsync(request);
                TempData["SuccessMessage"] = "SSS başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSS oluşturulurken bir hata oluştu.");
                TempData["ErrorMessage"] = "SSS oluşturulurken bir hata oluştu.";
                return View("~/Admin/Views/Faq/FaqCreate.cshtml", request);
            }
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqEdit(int id)
        {
            try
            {
                var faqCategories = await _faqCategoryClient.GetAllFaqCategoriesAsync();

                ViewBag.FaqCategories = faqCategories;

                var faq = await _faqClient.GetFaqByIdAsync(id);
                if (faq == null)
                {
                    TempData["ErrorMessage"] = "SSS bulunamadı.";
                    return RedirectToAction("FaqIndex");
                }
                return View(new UpdateFaqRequest
                {
                    Id = faq.Id,
                    Answer = faq.Answer,
                    Question = faq.Question,
                    CategoryId = faq.CategoryId
                }); // View pathi tam verildi
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSS düzenleme formu getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Bir hata oluştu.";
                return RedirectToAction("FaqIndex");
            }
        }


        [HttpPost]
        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqUpdate(int id, UpdateFaqRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Admin/Views/Faq/FaqEdit.cshtml", request);
            }

            try
            {
                await _faqClient.UpdateFaqAsync(id, request);
                TempData["SuccessMessage"] = "SSS başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSS güncellenirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqDelete(int id)
        {
            try
            {
                await _faqClient.DeleteFaqAsync(id);
                TempData["SuccessMessage"] = "SSS başarıyla silindi.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SSS silinirken bir hata oluştu.");
                TempData["ErrorMessage"] = "SSS silinirken bir hata oluştu.";
            }
            return RedirectToAction("Index");
        }
    }
}
