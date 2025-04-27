using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Bcpg.Sig;
using PlastikMVC.Client;
using PlastikMVC.Client.Models.Request.CategoryRequest;
using PlastikMVC.Client.Models.Response.CategoryResponse;
using PlastikMVC.Dtos.CategoryDto;
using PlastikMVC.Filters;


namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryClient _categoryClient;
        private readonly ProductClient _productClient;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryClient categoryClient, ProductClient productClient, ILogger<CategoryController> logger)
        {
            _categoryClient = categoryClient;
            _productClient = productClient;
            _logger = logger;
        }

        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var category = await _categoryClient.GetAllCategoriesAsync();
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategoriler getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Kategoriler yüklenirken bir hata oluştu. Lütfen tekrar deneyin.";
                return View(new List<GetAllCategoryResponse>()); //GetAllCategoryResponse burası böyle mi olmalı ??
            }
        }


        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                await _categoryClient.CreateCategoryAsync(request);
                TempData["SuccessMessage"] = "Kategori başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken bir hata oluştu.");
                TempData["ErrorMessage"] = "Kategori oluşturulurken bir hata oluştu.";
                return View(request);
            }
        }

        [HttpGet]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryClient.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index");
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori düzenleme formu getirilirken bir hata oluştu. ID: {Id}", id);
                TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }



        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Update(int id, UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Hatalyı bir model durumunda, tekrar formu doldurarak geri döndür
                try
                {
                    var products = await _productClient.GetProductByIdAsync(id);
                    ViewBag.Products = products;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ürün bilgileri getirilirken bir hata oluştu.");
                }

                return View(request); // Formdaki veriyi yeniden göstermek için request modelini kullan
            }

            try
            {
                // Kategori güncelleme işlemi
                await _categoryClient.UpdateCategoryAsync(id, request);

                // Başarı mesajını kullanıcıya ilet
                TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda logla ve kullanıcıya mesaj göster
                _logger.LogError(ex, "Kategori güncellenirken bir hata oluştu. ID: {Id}", id);
                TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }

        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {

                var category = await _categoryClient.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    TempData["ErrorMessage"] = "Kategori bulunamadı.";
                    return RedirectToAction("CategoryIndex"); // Kategorilerin listelendiği bir sayfaya yönlendirme
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama ve hata mesajı
                _logger.LogError(ex, "Kategori detayları getirilirken bir hata oluştu. ID: {Id}", id);
                TempData["ErrorMessage"] = "Kategori detayları yüklenirken bir hata oluştu.";
                return RedirectToAction("CategoryIndex");
            }
        }



        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryClient.GetCategoryByIdAsync(id); // Kategoriyi kontrol et
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Kategori bulunamadı.";
                    return RedirectToAction("Index");
                }

                await _categoryClient.DeleteCategoryAsync(id); // Kategoriyi sil

                TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
                return RedirectToAction("Delete");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kategori silinirken bir hata oluştu.";
                _logger.LogError(ex, "Kategori silinirken hata oluştu. ID: {Id}", id);
                return RedirectToAction("Index");
            }
        }

    }
}
