using PlastikMVC.AllDtos.ContentMediaDtos;
using System.ComponentModel.DataAnnotations;

namespace PlastikMVC.AllDtos.ContentDtos
{
    public class ContentDeleteDto
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
        public bool IsFeatured { get; set; }
        public string Summary { get; set; }
        public string ContentSource { get; set; }
        public DateTime ScheduledPublishDate { get; set; }
        public ICollection<ContentMediaDto> Medias { get; set; }
    }
}
