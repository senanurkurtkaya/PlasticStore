namespace PlastikMVC.Dtos.FaqCategoryDto
{
    public class FaqCategoryWithQuestionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FaqCategoryDto> Questions { get; set; } = new List<FaqCategoryDto>();
    }
}
