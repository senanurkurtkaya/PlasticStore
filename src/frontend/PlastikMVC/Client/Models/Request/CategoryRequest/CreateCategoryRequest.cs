using PlastikMVC.Dtos.ProductDto;
using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.Client.Models.Request.CategoryRequest
{
    public class CreateCategoryRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public  string Description { get; set; }

        public bool IsActive { get; set; }
        [Required]
        public int CategoryId { get; set; }

        //[Required]
        //public decimal Price { get; set; }

        [Required]
        public List<ProductImageDto> ProductImages { get; set; } = new();

    }
}
