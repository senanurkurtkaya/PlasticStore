using DAL.Enums;
using BLL.Dtos.CategoryDto;
using BLL.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ContentDto
{
    public class ContentGetAllDto: BaseDto
    { 
        public string Title { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string Slug { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }
        public string ContentSource { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string MediaUrl { get; set; }
        public DateTime ScheduledPublishDate { get; set; }
        public bool IsFeatured { get; set; }
        public List<BLL.Dtos.ContentMediaDto.ContentMediaDto> Medias { get; set; } = new List<BLL.Dtos.ContentMediaDto.ContentMediaDto>();

    }
}
