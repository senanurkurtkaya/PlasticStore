namespace PlastikMVC.Client.Models.Request.ProductRequest
{
    public class UpdateProductImageRequest
    {

        public int ProductId { get; set; }


        public string ImageUrl { get; set; }

        public string AltText { get; set; }

    
        public bool IsPreviewImage { get; internal set; }
        public int Id { get; internal set; }
    }
}
