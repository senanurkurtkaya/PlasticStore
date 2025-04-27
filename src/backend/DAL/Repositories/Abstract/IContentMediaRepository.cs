using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstract
{
    public interface IContentMediaRepository
    {
        Task<List<ContentMedia>> GetAllContentMediaAsync();
        Task<ContentMedia> GetContentMediaByIdAsync(int id);
        Task<List<ContentMedia>> GetFeaturedContentMediaAysnc();
        Task<List<ContentMedia>> GetLatestContentMediaAsync(int count);
        Task<List<ContentMedia>> GetAllMediasByContentIdAsync(int id);

    }
}
