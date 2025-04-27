using BLL.Dtos;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using BLL.Dtos.ProductDto;
namespace BLL.Dtos.ProductDto
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public bool IsAvailable => StockQuantity > 0;

        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        public string PreviewImageUrl { get; set; }

        public List<ProductImageDto> Images { get; set; }

        public ProductMetaTagsDto MetaTags { get; set; }

    }
}
