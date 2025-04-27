using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.Models
{
    public class ContentMedia : BaseEntity
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
        public string? AltText { get; set; }
        [MaxLength(500)]
        public string? Caption { get; set; }
    }
}
