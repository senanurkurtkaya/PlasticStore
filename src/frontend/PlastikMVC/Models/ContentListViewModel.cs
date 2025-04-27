using PlastikMVC.AllDtos.ContentDtos;

namespace PlastikMVC.Models
{
    public class ContentListViewModel
    {
        public List<ContentGetAllDto> AllContents { get; set; }
        public List<ContentGetAllDto> CategoryContents { get; set; }
        public List<ContentGetAllDto> FeaturedContents { get; set; }
        public List<ContentGetAllDto> LatestContents { get; set; }
        public List<ContentGetAllDto> PaginatedContents { get; set; }
    }
}
