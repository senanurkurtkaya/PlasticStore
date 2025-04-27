namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class ProductImageResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public bool IsPreviewImage { get; set; }
    }
}
