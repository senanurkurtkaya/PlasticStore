using PlastikMVC.Dtos.ProductDto;
using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.Client.Models.Request.ProductRequest
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }
   
        public List<ProductImageDto> ProductImages { get; set; }  = new List<ProductImageDto> ();

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
        public int StockQuantity { get; set; }

        public bool IsAvailable => StockQuantity > 0;


    }
}
