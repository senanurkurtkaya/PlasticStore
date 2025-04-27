using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Dtos.CategoryDto;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ContentDto
{
    public class ContentGetByIdDto :BaseDto
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContentStatus Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
        public string ContentSource { get; set; }
        public List<BLL.Dtos.ContentMediaDto.ContentMediaDto> Medias { get; set; } = new List<BLL.Dtos.ContentMediaDto.ContentMediaDto>();
       
    }
}
