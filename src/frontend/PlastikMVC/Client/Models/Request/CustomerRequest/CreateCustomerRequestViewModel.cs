namespace PlastikMVC.Client.Models.Request.CustomerRequest
{
    public class CreateCustomerRequestViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public string? UserId { get; set; }
    }
}
