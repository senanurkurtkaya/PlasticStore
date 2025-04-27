using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Dtos;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
namespace BLL.Dtos.ContentDto
{
    public class ContentAddDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }        
        public string Slug { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? PublishedDate { get; set; } = DateTime.Now;
        public DateTime ScheduledPublishDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType MediaType { get; set; }
        [JsonIgnore]
        public List<BLL.Dtos.ContentMediaDto.ContentMediaDto> Medias { get; set; } = new List<BLL.Dtos.ContentMediaDto.ContentMediaDto>();
        //[FromForm] 
        public List<IFormFile>? MediaFiles { get; set; }
        public string ContentSource { get; set; } = "Bilinmiyor";

    }
}
