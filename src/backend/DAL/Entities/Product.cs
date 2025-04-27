using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Product : BaseClass
    {

        public string Name { get; set; }
        public decimal Price { get; set; }
       
        public string Description { get; set; }
        public int CategoryId { get; set; } // Doğru tür
        public int StockQuantity { get; set; } // Doğru tür
        public bool IsActive { get; set; } // Doğru tür
        public List<ProductImage> Images
        {
            get; set;


        }
        public int QuantitySold { get; set; }
        public Category Category { get; set; } // Navigation Property
        public ProductMetaTags MetaTags { get; set; }

        public  int? MetaTagsId { get; set; }

    }
}
