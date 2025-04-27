using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class UpdateProductImageDto
    {

        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public bool IsPreviewImage { get; set; }
    }
}
