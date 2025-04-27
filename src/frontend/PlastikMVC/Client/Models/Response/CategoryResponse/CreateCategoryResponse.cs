using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.Client.Models.Response.CategoryResponse
{
    public class CreateCategoryResponse
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Ürün fiyatı sıfırdan büyük olmalıdır.")]
        //public decimal Price { get; set; }

        [Required(ErrorMessage = "CategoryId gereklidir.")]
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable => StockQuantity > 0;
        public int StockQuantity { get; set; }

    }
}
