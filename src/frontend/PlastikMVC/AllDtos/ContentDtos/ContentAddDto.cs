using Microsoft.AspNetCore.Mvc;
using PlastikMVC.AllDtos.ContentMediaDtos;
using PlastikMVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentDtos
{
    public class ContentAddDto:BaseDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }        
        public string Slug { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }
        public List<ContentMediaDto> Medias { get; set; } = new List<ContentMediaDto>();        
        public List<IFormFile>? MediaFiles { get; set; }
        public string ContentSource { get; set; } = "Bilinmiyor";
    }
}
