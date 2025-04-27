using PlastikMVC.AllDtos.ContentMediaDtos;
using PlastikMVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentDtos
{
    public class ContentDto :BaseDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContentStatus Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public ICollection<ContentMediaDto> Medias { get; set; }      
        public string ContentSource { get; set; }
        public bool IsFeatured { get; set; }
        public string MediaUrl { get; set; }
        public DateTime ScheduledPublishDate { get; set; }
    }
}
