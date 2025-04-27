namespace PlastikMVC.Client.Models.Response.CategoryResponse
{
    public class GetCategoryByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsAvailable => StockQuantity > 0;

        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int StockQuantity { get; set; }

    }
}
