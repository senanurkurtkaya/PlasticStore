namespace PlastikMVC.Client.Models.Request.FaqRequest
{
    public class CreateFaqRequest
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
