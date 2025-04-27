
using PlastikMVC.AllDtos.ContentDtos;
using PlastikMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentMediaDtos
{
    public class ContentMediaDto:BaseDto

    {
        public int ContentId { get; set; }
        public ContentDto Content { get; set; }
        public string Url { get; set; }
        public string? AltText { get; set; }
        public string? Caption { get; set; }
    }
}
