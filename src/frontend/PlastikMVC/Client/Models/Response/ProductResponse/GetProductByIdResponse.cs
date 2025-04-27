namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class GetProductByIdResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public  bool IsPreviewImage { get; set; }

        public string PreviewImageUrl { get; set; }

        public List<ProductImageResponse> Images { get; set; }

        public ProductMetaTagsResponse MetaTags { get; set; }

    }
}
