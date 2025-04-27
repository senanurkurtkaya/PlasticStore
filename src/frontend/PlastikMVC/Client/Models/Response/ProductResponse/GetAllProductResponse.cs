namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class GetAllProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }
        public bool IsActive { get; set; } = true;
    
        public int StockQuantity { get; set; }
        public bool IsAvailable => StockQuantity > 0;

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string PreviewImageUrl { get; set; }
    }
}
