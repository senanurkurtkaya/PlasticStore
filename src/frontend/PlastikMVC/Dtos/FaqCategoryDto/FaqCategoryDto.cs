namespace PlastikMVC.Dtos.FaqCategoryDto
{
    public class FaqCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
