using PlastikMVC.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.ProductDto
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public  string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int StockQuantity { get; set; }
        public List<string> ProductImages { get; set; } = new();

        public bool IsAvailable { get; set; }
    }
}
