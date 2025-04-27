using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class UpdateProductDto :BaseDto
    {
        public string Name { get; set; }   
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public int StockQuantity { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }


    }
}
