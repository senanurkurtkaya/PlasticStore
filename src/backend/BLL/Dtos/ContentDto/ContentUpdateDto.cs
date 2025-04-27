using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Dtos.ContentMediaDto;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BLL.Dtos.ContentDto
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
        public string ContentSource { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? ScheduledPublishDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }         
        public List<BLL.Dtos.ContentMediaDto.ContentMediaDto> Medias { get; set; } = new List<BLL.Dtos.ContentMediaDto.ContentMediaDto>();
    }
}
