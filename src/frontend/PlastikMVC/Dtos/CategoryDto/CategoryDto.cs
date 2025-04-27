using PlastikMVC.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.CategoryDto
{
    public class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string  CategoryId { get; set; }

        public int ProductCount { get; set; }

        //public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public string ImageUrl { get; set; }



    }
}
