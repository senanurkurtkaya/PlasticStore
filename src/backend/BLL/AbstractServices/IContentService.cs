using BLL.Dtos.ContentDto;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{     
    //ONR++ 29012025/22:20
    public interface IContentService
    {
        Task<ContentDto> AddContentAsync(ContentAddDto contentAddDto);
        Task<bool> DeleteContentAsync(int id);
        Task<bool> UpdateContentAsync(int id,ContentUpdateDto contentUpdateDto);
        Task<ContentGetByIdDto> GetContentByIdAsync(int id);
        Task<List<ContentGetAllDto>> GetAllContentAsync();
        Task<List<ContentGetByIdDto>> GetContentByCategoryAsync(int categoryId);
        Task<List<ContentGetAllDto>> GetFeaturedContentAsync();
        Task<List<ContentGetAllDto>> GetLatestContentAsync(int count);
        Task<List<ContentGetAllDto>> GetScheduledContentsAsync();
        Task<PagedResult<ContentGetAllDto>> GetPaginatedContentsAsync(int page, int pageSize);
        Task<ContentGetByIdDto> GetContentBySlug(string slug);
    }
}
