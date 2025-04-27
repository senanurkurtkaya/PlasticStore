using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.ContentMediaDtos
{
    public class ContentMediaDeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}
