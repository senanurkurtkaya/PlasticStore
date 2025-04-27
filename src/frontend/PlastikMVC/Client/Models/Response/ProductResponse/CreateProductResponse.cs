using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class CreateProductResponse
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; internal set; }

        public bool IsAvailable => StockQuantity > 0;

        public List<string> ProductImages { get; set; } = new();
    }
}
