using PlastikMVC.Dtos.CategoryDto;
using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Client;
using PlastikMVC.Client.Models.Request.ProductRequest;
using PlastikMVC.Models;
using PlastikMVC.Dtos.ProductDto;
using PlastikMVC.Models.Product;
using PlastikMVC.Client.Models.Response.ProductResponse;
using PlastikMVC.Exceptions;
using PlastikMVC.Filters;

namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly ProductClient _productClient;
        private readonly ILogger<ProductController> _logger;
        private readonly CategoryClient _categoryClient;

        public ProductController(ProductClient productClient, ILogger<ProductController> logger, CategoryClient categoryClient)
        {
            _productClient = productClient;
            _logger = logger;
            _categoryClient = categoryClient;
        }

        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            try
            {
                var products = await _productClient.GetAllProductsAsync();

                if (products == null || !products.Any())
                {
                    _logger.LogWarning("Ürünler API'den boş döndü!");
                    TempData["ErrorMessage"] = "Ürünler yüklenirken bir hata oluştu. Lütfen tekrar deneyin.";
                    return View(new List<GetAllProductResponse>());
                }

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
                return View(new List<GetAllProductResponse>());
            }

        }

        [HttpGet]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productClient.GetProductByIdAsync(id);
            var categories = await _categoryClient.GetAllCategoriesAsync();

            if (product == null || categories == null)
            {
                return NotFound("Ürün veya kategori bulunamadı.");
            }

            // `UpdateProductRequest` modeline dönüştür
            var updateRequest = new UpdateProductRequest
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,

            };

            if (TempData.Count > 0 && TempData.Keys.Any(k => k.Contains("Error")))
            {
                ViewBag.Errors = TempData.Where(x => x.Key.Contains("Error")).ToList();
            }

            ViewBag.Categories = categories;
            return View(updateRequest); // `UpdateProductRequest` gönderiliyor
        }



        [HttpPost]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Update(int id, UpdateProductViewModel productModel)
        {
            try
            {
                productModel.Id = id;

                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        if (state.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                        {
                            TempData[$"{state.Key}Error"] = $"{state.Key} Hatalı.";
                        }
                    }

                    return RedirectToAction(nameof(Edit), new { id });
                }

                var updateRequest = new UpdateProductRequest
                {
                    Id = productModel.Id,
                    Name = productModel.Name,
                    Description = productModel.Description,
                    Price = productModel.Price,
                    CategoryId = productModel.CategoryId,
                    IsActive = productModel.IsActive,
                    StockQuantity = productModel.StockQuantity
                };

                var updated = await _productClient.UpdateProductAsync(id, updateRequest);

                if (!updated)
                {
                    TempData["ErrorMessage"] = "Ürün güncellenemedi.";
                    return RedirectToAction("Edit", new { id });
                }

                TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }


        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Details(int id)
        {
            var products = await _productClient.GetAllProductsAsync();

            // Sadece tek bir ürün seç
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(product); // Model: GetProductByIdResponse
        }


        [HttpGet]
        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryClient.GetAllCategoriesAsync();
                ViewBag.Categories = categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving categories.");
                ViewBag.Categories = new List<CategoryDto>();  //Liste ile oluşturmak mantıklı mı
                ViewBag.Error = ex.Message; // Model: CreateProductRequest
            }

            return View();
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductRequest request, List<IFormFile> productImages)
        {

             if (productImages != null && productImages.Any())
            {
                foreach (var image in productImages)
                {
                    string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                         await image.CopyToAsync(stream);
                    }

                    request.ProductImages.Add(new ProductImageDto
                    {
                        FilePath = $"/Images/{fileName}",
                        FileName = fileName,
                        ImageUrl = $"/Images/{fileName}",
                        AltText = image.FileName,
                        IsPreviewImage = true
                    });
                }
            }

            try
            {
                await _productClient.CreateProductAsync(request);
                TempData["SuccessMessage"] = "Ürün başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün eklenirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Ürün eklenirken bir hata oluştu.";
                // return View(request);
                return RedirectToAction("Create");
            }

        }

        //[HttpPost]
        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)    // 1-)kullanıcaya silmek istiyortmsunuz diye göstermiyor .direk siliyor
        {
            try
            {

                await _productClient.DeleteProductAsync(id);


                TempData["SuccessMessage"] = "Ürün başarıyla silindi.";
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Ürün silme işlemi sırasında bir hata oluştu. ID: {Id}", id);
                TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
            }


            return RedirectToAction("Index");
        }

        // Tüm MetaTag'leri Listele
        [HttpGet("Admin/Product/{Id}/MetaTags")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> MetaTags(int Id)
        {
            try
            {
                var metaTagsResponse = await _productClient.GetMetaTagsByProductIdAsync(Id);

                // Eğer metaTagsResponse veya MetaTags boşsa, Index'e yönlendir
                if (metaTagsResponse == null)
                {
                    TempData["InfoMessage"] = "Bu ürün için henüz MetaTag eklenmemiş. Eklemek ister misiniz?";
                    ViewBag.ProductId = Id; // View'de kullanmak için ProductId gönderiyoruz.
                    return View("~/Areas/Admin/Views/Product/MetaTagIndex.cshtml");
                }

                return View("~/Areas/Admin/Views/Product/MetaTagIndex.cshtml", metaTagsResponse);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    ViewBag.ProductId = Id; // View'de kullanmak için ProductId gönderiyoruz.
                    return View("~/Areas/Admin/Views/Product/MetaTagIndex.cshtml");
                }

                _logger.LogError(ex, "MetaTag bilgileri getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = $"MetaTag bilgileri yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
        // MetaTag Oluşturma Sayfası
        [HttpGet("Admin/Product/{productId}/CreateMetaTag")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public IActionResult CreateMetaTag(int productId)
        {
            var request = new CreateProductMetaTagsRequest
            {
                ProductId = productId
            };
            return View(request); // Model: CreateProductMetaTagsRequest
        }

        // MetaTag Oluşturma
        [HttpPost("Admin/Product/{id}/CreateMetaTag")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> CreateMetaTag(CreateProductMetaTagsRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Geçersiz veri. Lütfen tekrar deneyin.";
                return View(request);
            }

            try
            {
                await _productClient.CreateMetaTagsAsync(request);
                TempData["SuccessMessage"] = "MetaTag başarıyla oluşturuldu.";
                return RedirectToAction(nameof(MetaTags), new { id = request.ProductId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MetaTag oluşturulurken bir hata oluştu.");
                TempData["ErrorMessage"] = "MetaTag oluşturulurken bir hata oluştu.";
                return View(request);
            }
        }

        // MetaTag Düzenleme
        [HttpGet("Admin/Product/{id}/MetaTagEdit")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> MetaTagEdit(int id)
        {
            try
            {
                // ✅ TEK BİR METATAG ALINMALI (metaTagId kullanıyoruz)
                var metaTag = await _productClient.GetMetaTagsByProductIdAsync(id);
                if (metaTag == null)
                {
                    TempData["ErrorMessage"] = "MetaTag bilgisi bulunamadı.";
                    return RedirectToAction("MetaTags");
                }

                var model = new ProductMetaTagsViewModel
                {
                    ProductId = metaTag.ProductId, // ✅ ProductId eklendi (API güncelleme için gerekli)
                    Title = metaTag.Title,
                    Description = metaTag.Description,
                    OpenGraphType = metaTag.OpenGraphType,
                    OpenGraphUrl = metaTag.OpenGraphUrl,
                    OpenGraphTitle = metaTag.OpenGraphTitle,
                    OpenGraphDescription = metaTag.OpenGraphDescription,
                    OpenGraphImage = metaTag.OpenGraphImage,
                    TwitterCard = metaTag.TwitterCard,
                    TwitterUrl = metaTag.TwitterUrl,
                    TwitterTitle = metaTag.TwitterTitle,
                    TwitterDescription = metaTag.TwitterDescription,
                    TwitterImage = metaTag.TwitterImage
                };

                return View("MetaTagEdit", model); // ✅ Doğru View adı
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MetaTag düzenleme sırasında bir hata oluştu.");
                TempData["ErrorMessage"] = "MetaTag düzenlenirken bir hata oluştu.";
                return RedirectToAction("MetaTags");
            }
        }

        [HttpPost("/Admin/Product/{id}/MetaTagEdit")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> UpdateMetaTag(int id, ProductMetaTagsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("MetaTagEdit", new { id });
            }

            var request = new UpdateProductMetaTagsRequest
            {

                ProductId = model.ProductId,
                Title = model.Title,
                Description = model.Description,
                OpenGraphType = model.OpenGraphType,
                OpenGraphUrl = model.OpenGraphUrl,
                OpenGraphTitle = model.OpenGraphTitle,
                OpenGraphDescription = model.OpenGraphDescription,
                OpenGraphImage = model.OpenGraphImage,
                TwitterCard = model.TwitterCard,
                TwitterUrl = model.TwitterUrl,
                TwitterTitle = model.TwitterTitle,
                TwitterDescription = model.TwitterDescription,
                TwitterImage = model.TwitterImage
            };

            var response = await _productClient.UpdateMetaTagAsync(model.MetaTagId, request);

            if (!response)
            {
                TempData["ErrorMessage"] = "MetaTag güncellenemedi.";
                return View(model);
            }
            TempData["SuccessMessage"] = "MetaTag başarıyla güncellendi.";
            return RedirectToAction("MetaTags", new { id = model.ProductId });
        }



        [HttpGet("/Admin/Product/{id}/ProductImages")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> GetProductImages(int id)
        {
            ViewBag.ProductId = id;
            try
            {
                var images = await _productClient.GetImagesByProductIdAsync(id);
                if (images == null || !images.Any())
                {
                    TempData["InfoMessage"] = "Bu ürün için henüz resim eklenmemiş.";
                    ViewBag.InfoMessage = "Bu ürün için henüz resim eklenmemiş.";
                }

                return View("~/Areas/Admin/Views/Product/ProductImagesIndex.cshtml", images.Select(x => new ProductImageViewModel
                {
                    Id = x.Id,
                    AltText = x.AltText,
                    ImageUrl = x.ImageUrl,
                    IsPreviewImage = x.IsPreviewImage,
                    ProductId = x.ProductId
                }));
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException)
                {
                    return View("~/Areas/Admin/Views/Product/ProductImagesIndex.cshtml");
                }

                _logger.LogError(ex, "Ürün resimleri getirilirken bir hata oluştu.");
                TempData["ErrorMessage"] = $"Ürün resimleri yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpGet("/Admin/Product/{id}/CreateProductImage")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public IActionResult CreateProductImage(int id)
        {
            var request = new CreateProductImageRequest { ProductId = id };
            return View(request);
        }


        [HttpPost]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> AddProductImage(CreateProductImageRequest request, IFormFile productImage)
        {


            if (productImage != null && productImage.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(productImage.FileName)}";
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                request.ImageUrl = $"/Images/{fileName}";
                ModelState.ClearValidationState(nameof(request.ImageUrl));
                ModelState.SetModelValue(nameof(request.ImageUrl), request.ImageUrl, request.ImageUrl);
                ModelState.MarkFieldValid(nameof(request.ImageUrl));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Geçersiz veri. Lütfen tekrar deneyin.";
                return View("CreateProductImage", request);
            }


            try
            {
                await _productClient.AddImageAsync(request);
                TempData["AddProductImageSuccessMessage"] = "Ürün resmi başarıyla eklendi.";
                return RedirectToAction(nameof(GetProductImages), new { id = request.ProductId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün resmi eklenirken bir hata oluştu.");
                TempData["AddProductImageErrorMessage"] = "Ürün resmi eklenirken bir hata oluştu.";
                return View("CreateProductImage", request);
            }
        }


        [HttpGet("/Admin/Product/{productId}/UpdateProductImage/{imageId}")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> EditProductImage(int productId, int imageId)
        {
            try
            {

                var imageList = await _productClient.GetImagesByProductIdAsync(productId);
                var image = imageList?.FirstOrDefault(i => i.Id == imageId);

                if (image == null)
                {
                    TempData["ErrorMessage"] = "Ürün resmi bulunamadı.";
                    return RedirectToAction("GetProductImages", new { productId });
                }

                var model = new UpdateProductImageRequest
                {
                    Id = image.Id,
                    ProductId = image.ProductId,
                    ImageUrl = image.ImageUrl,
                    AltText = image.AltText,
                    IsPreviewImage = image.IsPreviewImage
                };

                return View("EditProductImage", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün resmi düzenleme sırasında bir hata oluştu.");
                TempData["ErrorMessage"] = "Ürün resmi düzenlenirken hata oluştu.";
                return RedirectToAction("GetProductImages", new { productId });
            }
        }


        [HttpPost("/Admin/Product/{productId}/UpdateProductImage/{imageId}")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> UpdateProductImage(int productId, int imageId, UpdateProductImageRequest request, IFormFile productImage)
        {
            if (productImage != null && productImage.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(productImage.FileName)}";
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                request.ImageUrl = $"/Images/{fileName}";
                ModelState.ClearValidationState(nameof(request.ImageUrl));
                ModelState.SetModelValue(nameof(request.ImageUrl), request.ImageUrl, request.ImageUrl);
                ModelState.MarkFieldValid(nameof(request.ImageUrl));
            }

            if (!ModelState.IsValid)
            {
                return View("EditProductImage", request);
            }

            try
            {
                var success = await _productClient.UpdateImageAsync(productId, imageId, request);

                if (!success)
                {
                    TempData["ErrorMessage"] = "Ürün resmi güncellenemedi.";
                    return View("EditProductImage", request);
                }

                TempData["SuccessMessage"] = "Ürün resmi başarıyla güncellendi.";
                return RedirectToAction("GetProductImages", new { id = productId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün resmi güncellenirken bir hata oluştu.");
                TempData["ErrorMessage"] = "Ürün resmi güncellenirken bir hata oluştu.";
                return RedirectToAction("EditProductImage", new
                {
                    productId,
                    imageId
                });
                //return View("EditProductImage", request);
            }
        }


        [HttpPost("{imageId}/Delete")]
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> DeleteProductImage(int productId, int imageId)
        {
            try
            {
                await _productClient.DeleteImageAsync(productId, imageId);
                TempData["SuccessMessage"] = "Ürün resmi başarıyla silindi.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün resmi silme sırasında bir hata oluştu.");
                TempData["ErrorMessage"] = "Ürün resmi silinirken bir hata oluştu.";
            }

            return RedirectToAction("GetProductImages", new { id = productId });
        }
    }

}
