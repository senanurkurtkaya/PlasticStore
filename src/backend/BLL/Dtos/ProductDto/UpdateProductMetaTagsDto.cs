using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class UpdateProductMetaTagsDto
    {
 
        public string Title { get; set; }
        public string Description { get; set; }

        // Open Graph Bilgileri
        public string OpenGraphType { get; set; }
        public string OpenGraphUrl { get; set; }
        public string OpenGraphTitle { get; set; }
        public string OpenGraphDescription { get; set; }
        public string OpenGraphImage { get; set; }

        // Twitter Card Bilgileri
        public string TwitterCard { get; set; }
        public string TwitterUrl { get; set; }
        public string TwitterTitle { get; set; }
        public string TwitterDescription { get; set; }
        public string TwitterImage { get; set; }
    }
}
