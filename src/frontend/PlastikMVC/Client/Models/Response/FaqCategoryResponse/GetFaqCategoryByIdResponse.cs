using PlastikMVC.Client.Models.Response.FaqResponse;

namespace PlastikMVC.Client.Models.Response.FaqCategoryResponse
{
    public class GetFaqCategoryByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetAllFaqResponse> Questions { get; set; }
    }
}
