using DAL.Entities;
using DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL.Dtos.ContentMediaDto
{
    public class ContentMediaGetByIdDto :BaseDto
    {
        [Required]
        public int ContentId { get; set; }
        public Content Content { get; set; }
        [Required]
        [MaxLength(500)]
        public string Url { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaType Type { get; set; }
        [MaxLength(250)]
        public string AltText { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }
    }
}
