using BLL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.CategoryDto
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable => StockQuantity > 0;
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }
        public int CategoryId { get; set; }


        public decimal Price { get; set; }


    }
}
