using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.ProductDto
{
    public class CreateProductDto :BaseDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }


    }
}
