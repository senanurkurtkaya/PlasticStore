namespace PlastikMVC.Models.Product
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
        public int StockQuantity { get; set; }
    }
}
