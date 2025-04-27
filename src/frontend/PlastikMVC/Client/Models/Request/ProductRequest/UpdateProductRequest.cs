using PlastikMVC.Dtos.ProductDto;
using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.Client.Models.Request.ProductRequest
{
    public class UpdateProductRequest 
    {

        public  int  Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } 
        public string Description { get; set; }



        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
   
        public List<ProductImageDto> ProductImages { get; set; } = new();

    }
}
