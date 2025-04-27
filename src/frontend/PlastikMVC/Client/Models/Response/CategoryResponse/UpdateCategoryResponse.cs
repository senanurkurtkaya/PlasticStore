namespace PlastikMVC.Client.Models.Response.CategoryResponse
{
    public class UpdateCategoryResponse
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable => StockQuantity > 0;
    }
}
