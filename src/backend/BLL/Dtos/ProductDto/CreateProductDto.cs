using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class CreateProductDto 
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<ProductImageDto> ProductImages { get; set; } 

        public bool IsAvailable => StockQuantity > 0;
        [Required]
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}
