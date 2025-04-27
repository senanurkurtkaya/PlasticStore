using DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Content : BaseClass
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
        [JsonIgnore]
        public ICollection<ContentMedia> Medias { get; set; }
        [MaxLength(250)]
        public string ContentSource { get; set; } = "Bilinmiyor";
        public bool IsFeatured { get; set; }
        public DateTime ScheduledPublishDate { get; set; }
    }
}
