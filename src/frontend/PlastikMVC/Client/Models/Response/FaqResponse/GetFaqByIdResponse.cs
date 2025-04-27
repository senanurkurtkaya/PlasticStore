namespace PlastikMVC.Client.Models.Response.FaqResponse
{
    public class GetFaqByIdResponse
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
