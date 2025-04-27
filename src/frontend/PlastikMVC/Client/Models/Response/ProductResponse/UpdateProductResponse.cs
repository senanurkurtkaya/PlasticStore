namespace PlastikMVC.Client.Models.Response.ProductResponse
{
    public class UpdateProductResponse
    {


        public string Name { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int StockQuantity { get; set; }

        public int Id { get; set; }

        public bool? IsActive { get; set; }
    }
}
