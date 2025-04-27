using BLL.AbstractServices;
using BLL.Dtos.ContentDto;
using BLL.Dtos.ContentMediaDto;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService ?? throw new ArgumentNullException(nameof(contentService));
    }
   
    [HttpPost("CreateContent")]
    public async Task<IActionResult> CreateContent([FromForm] ContentAddDto contentAddDto,[FromForm]List<ContentMediaDto> medias)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        contentAddDto.Medias = medias;

        await _contentService.AddContentAsync(contentAddDto);

        return Ok(new { message = "İçerik başarıyla eklendi!" });
    }

    [HttpPut("UpdateContent/{id}")]
    public async Task<IActionResult> UpdateContent(int id, [FromForm] ContentUpdateDto contentUpdateDto)
    {
        try
        {
            var updated = await _contentService.UpdateContentAsync(id, contentUpdateDto);
            if (!updated)
            {
                return BadRequest(new { error = "İçerik güncellenirken bir hata oluştu!" });
            }

            return Ok(new { message = "İçerik başarıyla güncellendi!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("DeleteContent/{id}")]
    public async Task<IActionResult> DeleteContent(int id)
    {
        try
        {
            await _contentService.DeleteContentAsync(id);
            return Ok(new { message = "İçerik başarıyla silindi!" });
            
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("GetAllContents")]
    public async Task<ActionResult<List<ContentGetAllDto>>> GetAllContents()
    {
        try
        {
            var contents = await _contentService.GetAllContentAsync();
            return Ok(contents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("GetContentByCategory/{id}")]
    public async Task<ActionResult<List<ContentGetAllDto>>> GetContentByCategoryAsync(int id)
    {
        try
        {
            var contents = await _contentService.GetContentByCategoryAsync(id);
            return contents.Any() ? Ok(contents) : NotFound(new { error = "İçerik bulunamadı!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
   
    [HttpGet("GetLatestContent/{count}")]
    public async Task<ActionResult<List<ContentGetAllDto>>> GetLatestContentAsync(int count)
    {
        try
        {
            var contents = await _contentService.GetLatestContentAsync(count);
            return contents.Any() ? Ok(contents) : NotFound(new { error = "İçerik bulunamadı!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("GetPaginatedContents/{page}/{pageSize}")]
    public async Task<ActionResult<PagedResult<ContentGetAllDto>>> GetPaginatedContentsAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        try
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new { error = "Sayfa numarası ve sayfa boyutu sıfırdan büyük olmalıdır!" });
            }

            var paginatedContents = await _contentService.GetPaginatedContentsAsync(page, pageSize);
            return Ok(paginatedContents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("GetContentById/{id}")]
    public async Task<IActionResult> GetContentById(int id)
    {
        try
        {
            var content = await _contentService.GetContentByIdAsync(id);

            if (content == null)
                return NotFound(new { error = "Böyle bir içerik yoktur!" });

            return Ok(content);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("GetScheduledContents")]
    public async Task<ActionResult<List<ContentGetAllDto>>> GetScheduledContentsAsync()
    {
        try
        {
            var scheduledContents = await _contentService.GetScheduledContentsAsync();
            return Ok(scheduledContents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("GetFeaturedContents")]
    public async Task<ActionResult<List<ContentGetAllDto>>> GetFeaturedContents()
    {
        try
        {
            var featuredContents = await _contentService.GetFeaturedContentAsync();
            return Ok(featuredContents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpGet("GetContentBySlug/{slug}")]
    public async Task<IActionResult> GetContentBySlug(string slug)
    {
        try
        {
            var content = await _contentService.GetContentBySlug(slug);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
