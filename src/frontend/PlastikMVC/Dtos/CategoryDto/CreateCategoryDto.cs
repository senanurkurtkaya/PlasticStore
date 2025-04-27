using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.CategoryDto
{
    public class CreateCategoryDto :BaseDto
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
    }
}
