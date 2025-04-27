using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string AltText { get; set; }

        public bool IsPreviewImage {  get; set; }
    }
}
