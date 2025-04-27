using BLL.AbstractServices;
using BLL.Dtos.ContentMediaDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlastikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentMediaController : ControllerBase
    {
        private readonly IContentMediaService _contentMediaService;

        public ContentMediaController(IContentMediaService contentMediaService)
        {
            _contentMediaService = contentMediaService;
        }
        [HttpPost("CreateContentMedia")]
        public async Task<IActionResult> CreateContentMedia(ContentMediaAddDto contentMediaAddDto)
        {
            try
            {
                await _contentMediaService.AddContentMediaAsync(contentMediaAddDto);

                return Ok(new {message = "İçerik medyası başarıyla eklendi!"});
                
            }

            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedia(int id, [FromBody] ContentMediaUpdateDto mediaDto)
        {
            try
            {
                await _contentMediaService.UpdateContentMediaAsync(id, mediaDto);
                return Ok(new { message = "Medya başarıyla güncellendi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            try
            {
                await _contentMediaService.DeleteContentMediaAsync(id);
                return Ok(new { message = "Medya başarıyla silindi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaById(int id)
        {
            try
            {
                var media = await _contentMediaService.GetContentMediaByIdAsync(id);

                if (media == null)
                    return NotFound(new { error = "Medya bulunamadı!" });

                return Ok(media);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

       
        [HttpGet("GetFeaturedContentMedia")]
        public async Task<IActionResult> GetFeaturedContentMedias()
        {
            try
            {
                var medias = await _contentMediaService.GetFeaturedContentMediaAysnc();

                return Ok(medias);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }     
        [HttpGet("GetLatestContentMedias/{count}")]
        public async Task<IActionResult> GetLatestContentMedias(int count)
        {
            try
            {
                var medias = await _contentMediaService.GetLatestContentMediaAsync(count);

                return Ok(medias);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        [HttpGet("GetAllMediasByContentId/{id}")]
        public async Task<IActionResult> GetAllMediasByContentId(int id)
        {
            try
            {
                var medias = await _contentMediaService.GetAllMediasByContentIdAsync(id);

                return Ok(medias);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
