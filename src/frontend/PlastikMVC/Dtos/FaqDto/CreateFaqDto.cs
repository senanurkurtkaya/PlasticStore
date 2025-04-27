namespace PlastikMVC.Dtos.FaqDto
{
    public class CreateFaqDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
