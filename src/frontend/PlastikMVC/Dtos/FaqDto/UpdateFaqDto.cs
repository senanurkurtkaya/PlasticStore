namespace PlastikMVC.Dtos.FaqDto
{
    public class UpdateFaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
