using Microsoft.AspNetCore.Mvc.Formatters;
using PlastikMVC.AllDtos.ContentDtos;
using PlastikMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlastikMVC.AllDtos.ContentMediaDtos
{
    public class ContentMediaUpdateDto:BaseDto
    {
        public int ContentId { get; set; }
        public ContentDto Content { get; set; }
        public string Url { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public MediaType Type { get; set; }
        public string? AltText { get; set; }  
        public string? Caption { get; set; }
    }
}
