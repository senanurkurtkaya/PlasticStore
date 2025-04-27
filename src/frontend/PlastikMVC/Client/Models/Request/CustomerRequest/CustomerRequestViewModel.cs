namespace PlastikMVC.Client.Models.Request.CustomerRequest
{
    public class CustomerRequestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ProposalDetails { get; set; }

        public decimal? EstimatedPrice { get; set; }
    }

}
