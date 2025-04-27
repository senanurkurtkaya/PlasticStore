using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client.Models.Request.FaqCategoryRequest;
using PlastikMVC.Client.Models.Response.FaqCategoryResponse;
using PlastikMVC.Client;
using Microsoft.AspNetCore.Authorization;
using PlastikMVC.Filters;

namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaqCategoryController : Controller
    {
        private readonly FaqCategoryClient _faqCategoryClient;
        private readonly ILogger<FaqCategoryController> _logger;

        public FaqCategoryController(FaqCategoryClient faqCategoryClient, ILogger<FaqCategoryController> logger)
        {
            _faqCategoryClient = faqCategoryClient;
            _logger = logger;
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _faqCategoryClient.GetAllFaqCategoriesAsync();
                return View(categories); // View pathi tam verildi
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FAQ kategorileri getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Kategoriler yüklenirken bir hata oluştu.";
                return View(new List<GetAllFaqCategoryResponse>());
            }
        }

        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public IActionResult FaqCategoryCreate()
        {
            return View();
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCategoryCreate(CreateFaqCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Admin/Views/FaqCategory/FaqCategoryCreate.cshtml", request);
            }

            try
            {
                await _faqCategoryClient.CreateFaqCategoryAsync(request);
                TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken hata oluştu.");
                TempData["FaqCategoryCreateErrorMessage"] = "Kategori oluşturulurken hata oluştu.";
                return View("~/Admin/Views/FaqCategory/FaqCategoryCreate.cshtml", request);
            }
        }


        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCategoryEdit(int id)
        {
            try
            {
                var category = await _faqCategoryClient.GetFaqCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index");
                }
                return View(new UpdateFaqCategoryRequest
                {
                    Id = category.Id,
                    Name = category.Name
                }); // View pathi tam verildi
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori düzenleme formu getirilirken hata oluştu.");
                TempData["ErrorMessage"] = "Bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCategoryUpdate(int id, UpdateFaqCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Admin/Views/FaqCategory/FaqCategoryEdit.cshtml", request);
            }

            try
            {
                await _faqCategoryClient.UpdateFaqCategoryAsync(id, request);
                TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori güncellenirken hata oluştu.");
                TempData["FaqCategoryUpdateErrorMessage"] = "Bir hata oluştu.";
                return View("~/Admin/Views/FaqCategory/FaqCategoryEdit.cshtml");
            }
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,Müşteri Hizmetleri")]
        public async Task<IActionResult> FaqCategoryDelete(int id)
        {
            try
            {
                await _faqCategoryClient.DeleteFaqCategoryAsync(id);
                TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori silinirken hata oluştu.");
                TempData["ErrorMessage"] = "Kategori silinirken hata oluştu.";
            }
            return RedirectToAction("Index");
        }
    }
}
