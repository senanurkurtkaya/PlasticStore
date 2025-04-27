namespace PlastikMVC.Client.Models.Request.FaqRequest
{
    public class UpdateFaqRequest
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
