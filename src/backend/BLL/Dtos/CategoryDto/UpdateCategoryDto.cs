using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Dtos.CategoryDto
{
    public class UpdateCategoryDto :BaseDto
    {
       
       
        public string Name { get; set; }
        public string Description { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable => StockQuantity > 0;
    }
}
