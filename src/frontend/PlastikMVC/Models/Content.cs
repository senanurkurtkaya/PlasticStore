using Microsoft.AspNetCore.Mvc.Formatters;
using PlastikMVC.Enums;
using PlastikMVC.Models.Category;
using PlastikMVC.Models.Product;
using System.ComponentModel.DataAnnotations;
      using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json.Serialization;
namespace PlastikMVC.Models
{
    public class Content :BaseEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public PlastikMVC.Enums.MediaType Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContentStatus Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public ICollection<ContentMedia> Medias { get; set; }
        [MaxLength(250)]
        public string ContentSource { get; set; }
        public bool IsFeatured { get; set; }
        public string MediaUrl { get; set; }
        public DateTime ScheduledPublishDate { get; set; }
    }
}
