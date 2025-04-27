
using PlastikMVC.AllDtos.ContentMediaDtos;
using PlastikMVC.Dtos.CategoryDto;
using PlastikMVC.Dtos.ProductDto;
using PlastikMVC.Enums;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentDtos
{
    public class ContentGetAllDto:BaseDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string ContentSource { get; set; }
        public string Slug { get; set; }
        public DateTime? PublishedDate { get; set; }        
        public DateTime ScheduledPublishDate { get; set; }
        public bool IsFeatured { get; set; }
        public List<ContentMediaDto> Medias { get; set; } = new List<ContentMediaDto>();
    }
}
