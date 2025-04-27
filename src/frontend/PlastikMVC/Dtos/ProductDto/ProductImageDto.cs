
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlastikMVC.Dtos.ProductDto
{
    public class ProductImageDto :BaseDto
    {

        public string FilePath { get; set; } // Statik dosya yolu
        public string FileName { get; set; } // Dosya adı
        public string ContentType { get; set; } // İçerik tipi (ör. image/jpeg)
        public string ImageUrl { get; set; }
        public bool IsPreviewImage { get; set; }
        public string AltText { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new List<ProductImageDto>();
    }
}
