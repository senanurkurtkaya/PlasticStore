namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class GetTop20ProductResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string PreviewImageUrl { get; set; }
    }
}
