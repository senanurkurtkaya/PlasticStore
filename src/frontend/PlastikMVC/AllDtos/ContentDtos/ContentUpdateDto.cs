using PlastikMVC.AllDtos.ContentMediaDtos;
using PlastikMVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentDtos
{
    public class ContentUpdateDto
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Slug { get; set; }
        [MaxLength(500)]
        public string Summary { get; set; }
        public string Body { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContentStatus Status { get; set; }
        public string ContentSource { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }
        public List<ContentMediaDto> Medias { get; set; } = new List<ContentMediaDto>();
    }
}
