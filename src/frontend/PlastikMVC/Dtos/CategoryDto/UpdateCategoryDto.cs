using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.CategoryDto
{
    public class UpdateCategoryDto  :BaseDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
