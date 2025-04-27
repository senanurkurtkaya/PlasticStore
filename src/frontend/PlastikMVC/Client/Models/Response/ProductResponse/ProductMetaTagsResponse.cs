using PlastikMVC.Dtos.ProductDto;

namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class ProductMetaTagsResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OpenGraphType { get; set; }
        public string OpenGraphUrl { get; set; }
        public string OpenGraphTitle { get; set; }
        public string OpenGraphDescription { get; set; }
        public string OpenGraphImage { get; set; }
        public string TwitterCard { get; set; }
        public string TwitterUrl { get; set; }
        public string TwitterTitle { get; set; }
        public string TwitterDescription { get; set; }
        public string TwitterImage { get; set; }
    }
}
