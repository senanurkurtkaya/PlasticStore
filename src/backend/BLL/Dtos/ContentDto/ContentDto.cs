using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Dtos.ContentDto
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
        public DateTime? PublishedDate { get; set; } = DateTime.Now;
        public ICollection<BLL.Dtos.ContentMediaDto.ContentMediaDto> Medias { get; set; }
        [MaxLength(250)]
        public string ContentSource { get; set; }
        public bool IsFeatured { get; set; }
        public string MediaUrl { get; set; }
        public DateTime ScheduledPublishDate { get; set; }

    }
}
