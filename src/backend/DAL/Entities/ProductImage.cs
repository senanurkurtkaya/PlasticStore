
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class ProductImage : BaseClass
    {
       
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string AltText { get; set; }

        public bool IsPreviewImage { get; set; } 


    }
}
