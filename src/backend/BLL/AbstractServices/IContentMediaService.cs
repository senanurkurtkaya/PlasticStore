using BLL.Dtos.ContentMediaDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IContentMediaService
    {
        Task AddContentMediaAsync(ContentMediaAddDto contentMediaAddDto);
        Task DeleteContentMediaAsync(int id);
        Task UpdateContentMediaAsync(int id,ContentMediaUpdateDto contentMediaUpdateDto);
        Task<ContentMediaDto> GetContentMediaByIdAsync(int id);
        Task<List<ContentMediaDto>> GetFeaturedContentMediaAysnc();
        Task<List<ContentMediaDto>> GetLatestContentMediaAsync(int count);
        Task<List<ContentMediaDto>> GetAllMediasByContentIdAsync(int id);
    }
}
