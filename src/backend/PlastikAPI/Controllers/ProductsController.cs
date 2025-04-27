using BLL.AbstractServices;
using BLL.ConcreteServices;
using BLL.Dtos.ProductDto;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace PlastikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductMetaTagsService _productMetaTagsService;
        private readonly IProductImageService _productImageService;


        public ProductsController(IProductService productService, ICategoryService categoryService, IProductMetaTagsService productMetaTagsService, IProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _productMetaTagsService = productMetaTagsService;
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await _productService.CreateAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
         }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,İçerikYöneticisi")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            // DTO içindeki Id alanını URL'deki id ile set et
            updateProductDto.Id = id;

            // ID kontrolü
            if (id != updateProductDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            // Güncelleme işlemi
            var result = await _productService.UpdateAsync(id, updateProductDto);

            if (result == null)
            {
                return NotFound();
            }

            return NoContent(); // Başarılı bir şekilde güncellenmişse 204 No Content döner
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/metaTags")]
        public async Task<IActionResult> GetMetaTags(int id)
        {
            var result =  await _productMetaTagsService.GetByProductIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("{id}/MetaTags")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> CreateMetaTags(int id, CreateProductMetaTagsDto createProductMetaTagsDto)
        {
            createProductMetaTagsDto.ProductId = id;
            await _productMetaTagsService.CreateAsync(createProductMetaTagsDto);

            return Ok();
        }

    
        [HttpPut("{id}/MetaTags")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> UpdateMetaTags(int id, [FromBody] UpdateProductMetaTagsDto updateProductMetaTagsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _productMetaTagsService.GetByProductIdAsync(id);  //şimdi burada GetByIdAsyn kullandık diger denediigmde de productByıd idi unutma sor ??? productId Mİ gelmeli metatagId Mİ
            if (entity == null)
            {
                return NotFound("MetaTag bulunamadı.");
            }

            await _productMetaTagsService.UpdateAsync(id, updateProductMetaTagsDto);

            return Ok("MetaTag başarıyla güncellendi.");
        }

      
        [HttpDelete("{id}/MetaTags")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult> DeleteMetaTags(int id)
        {
            await _productMetaTagsService.DeleteAsync(id);
            return NoContent();
        }

        //_---------------------------------------------
        
        [HttpGet("{productId}/ProductImages")]
        public async Task<IActionResult> GetImages(int productId) //belirli bir ürübne ait resimleri getir. (prıductId ye göre)
        {
            var images = await _productImageService.GetImagesByProductIdAsync(productId);
            if (images == null || !images.Any()) return NotFound($"böyle bir resim yok");
            return Ok(images);
        }

        // Belirli bir ürüne yeni yerni  resim ekle Hangi ürün için eklenecek ? (ProductId) :POST /api/Products/5/ProductImages
        [HttpPost("{productId}/ProductImages")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult>AddImage(int productId, [FromBody] CreateProductImageDto createProductImageDto)  
        {
            var image = await _productImageService.AddImageAsync(productId, createProductImageDto);
            return CreatedAtAction(nameof(AddImage), new {productId},image);
        }

        // belirli bir ürüne ait belirli bir resmi güncellicek  //productId : Hangi ütünr ait resim güncellencek ? 
        //imageId: hangi resim güncellencek? ör: PUT /api/Products/5/ProductImages/10
        [HttpPut("{productId}/ProductImages/{imageId}")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult>UpdateImage(int productId , int imageId, [FromBody] UpdateProductImageDto updateProductImageDto)
        {
            var success = await _productImageService.UpdateImageAsync(productId , imageId, updateProductImageDto);
            if (!success) return NotFound($"Bu ıd ye sahip  resim bulunamadı");

            return NoContent();
        }

        //belirli bir ürüne ait belirli bir resmi silicek
        // ProductId hangi ürünü ?    imageId: Hangi resim?   DELETE /api/Products/5/ProductImages/10
        [HttpDelete("{productId}/ProductImages/{imageId}")]
        [Authorize(Roles = "Admin,İçerik Yöneticisi")]
        public async Task<IActionResult>DeleteImage(int productId , int imageId)
        {
            var success = await _productImageService.DeleteImageAsync(productId, imageId);
            if (!success) return NotFound($"Image with Id: {imageId} not found for ProductId: {productId}");

            return NoContent();
        }

        [HttpGet("Top20")]
        public async Task<IActionResult> Top20()
        {
            var products = await _productService.GetTop20Async();

            return Ok(products);
        }
    }
}

