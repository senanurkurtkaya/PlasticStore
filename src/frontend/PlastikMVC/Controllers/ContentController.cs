using Microsoft.AspNetCore.Mvc;
using PlastikMVC.AllDtos.ContentDtos;
using PlastikMVC.AllDtos.ContentMediaDtos;
using PlastikMVC.Filters;
using PlastikMVC.Models;
namespace PlastikMVC.Controllers
{
    public class ContentController : Controller
    {
        private readonly HttpClientForContent _contentService;

        public ContentController(HttpClientForContent httpClientForContent)
        {
            _contentService = httpClientForContent;
        }
        
        public async Task<IActionResult> Index()
        {
            var contents = await _contentService.GetAllContentsAsync();
            return View(contents);
        }
       
        public async Task<IActionResult> Details(int id)
        {
            var content = await _contentService.GetContentByIdAsync(id);
            
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public IActionResult Create()
        {
            return View();
        }
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ContentAddDto contentDto,[FromForm] List<IFormFile> files)
        {
            
            if (!ModelState.IsValid)
            {
                return View(contentDto);
            }

            contentDto.MediaFiles = files;

            var success = await _contentService.CreateContentAsync(contentDto);

            if (success) return RedirectToAction("Index");

            ModelState.AddModelError("", "İçerik eklenirken hata oluştu.");
            return View(contentDto);

        }
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var content = await _contentService.GetContentByIdAsync(id);

            if (content == null)
            {
                return NotFound();
            }

            var updateContent = new ContentUpdateDto
            {
                //Id= content.Id,
                Body = content.Body,
                Title = content.Title,
                Slug = content.Slug,
                Summary = content.Summary,
                IsFeatured = content.IsFeatured,
                ScheduledPublishDate = content.ScheduledPublishDate,
                ContentSource = content.ContentSource,
                Medias = content.Medias.Select(m => new ContentMediaDto
                {
                    Id = m.Id,
                    Url = m.Url,
                    AltText = m.AltText
                }).ToList() ?? new List<ContentMediaDto>()
            };


            return View(updateContent);
        }

        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id,[FromForm] ContentUpdateDto contentUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return View(contentUpdateDto);
            }

           
            if (contentUpdateDto.Id != 0 && contentUpdateDto.Id != id)
            {
                ModelState.AddModelError("", "Gönderilen içerik ID'si geçersiz!");
                return View(contentUpdateDto);
            }

            bool success = await _contentService.UpdateContentAsync(id, contentUpdateDto);
            if (!success)
            {
                ModelState.AddModelError("", "İçerik güncellenemedi!");
                return View(contentUpdateDto);
            }

            return RedirectToAction(nameof(Index));

        }
        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> Delete(int id)
        {
            var content = await _contentService.GetContentByIdAsync(id);

            if (content == null)
            {
                return NotFound();
            }

            var contentDeleteDto = new ContentDeleteDto
            {
                Id = content.Id,
                Title = content.Title,
                Body = content.Body,
                Summary = content.Summary,
                ContentSource = content.ContentSource,
                ScheduledPublishDate = content.ScheduledPublishDate.Value
            };

            return View(contentDeleteDto);
        }

        [PlastikAuth(Roles = "Admin,İçerik Yöneticisi")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool success = await _contentService.DeleteContentAsync(id);

           
            if (!success)
            {
                ModelState.AddModelError("", "Silme işlemi başarısız!");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ListAllReports(string filter, int? id, int? count, int? page, int? pageSize)
        {
            var contents = new ContentListViewModel();

            switch (filter)
            {
                case "category":

                    if (id.HasValue)
                    {

                        var getCategory = await _contentService.GetContentsByCategoryAsync(id.Value);

                        contents.CategoryContents = getCategory;

                        return View("ListAllReports", contents);

                    }
                    break;

                case "featured":

                    var getFeatured = await _contentService.GetFeaturedContentsAsync();

                    contents.FeaturedContents = getFeatured;

                    return View("ListAllReports", contents);


                case "latestContent":

                    if (count.HasValue)
                    {
                        var getLatest = await _contentService.GetLatestContentsAsync(count.Value);

                        contents.LatestContents = getLatest;
                        return View("ListAllReports", contents);

                    }
                    break;
                case "pagedContents":

                    if (page.HasValue && pageSize.HasValue)
                    {
                        var paginatedResult = await _contentService.GetPaginatedContentsAsync(page.Value, pageSize.Value);

                        contents.PaginatedContents = paginatedResult.Items;

                        return View("ListAllReports", contents);
                    }
                    break;
                default:
                    contents.AllContents = await _contentService.GetAllContentsAsync();
                    break;

            }

            return View("ListAllReports", contents);
        }
    }
}

