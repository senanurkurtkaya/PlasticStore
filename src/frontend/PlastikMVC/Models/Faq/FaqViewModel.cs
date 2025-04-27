using PlastikMVC.Client.Models.Response.FaqCategoryResponse;
using PlastikMVC.Client.Models.Response.FaqResponse;

namespace PlastikMVC.Models.Faq
{
    public class FaqViewModel
    {
      
        public List<GetAllFaqCategoryResponse> FaqCategories { get; set; }
        public GetAllFaqCategoryResponse SelectedCategory { get; set; }
        public List<GetAllFaqResponse> Faqs { get; internal set; }
    }
}
