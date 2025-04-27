using PlastikMVC.Dtos.ProductDto;

namespace PlastikMVC.Client.Models.Request.ProductRequest
{
    public class CreateProductImageRequest
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPreviewImage { get; set; }
        public string AltText { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
    }
}
