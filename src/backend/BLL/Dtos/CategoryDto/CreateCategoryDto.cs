
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CategoryDto
{
    public class CreateCategoryDto 
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsActive { get; set; }

        [Required]
        public int CategoryId { get; set; }

      

        //[Required]
        //public List<ProductImage> ProductImages { get; set; } = new();

        public int StockQuantity { get; set; }

        public bool IsAvailable => StockQuantity > 0;
    }
}
