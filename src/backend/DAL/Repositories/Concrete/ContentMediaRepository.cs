using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Concrete
{
    public class ContentMediaRepository : IContentMediaRepository
    {
        private readonly AppDbContext _db;

        public ContentMediaRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ContentMedia>> GetAllContentMediaAsync()
        {
            return await _db.ContentMedias.ToListAsync();
        }

        public async Task<List<ContentMedia>> GetAllMediasByContentIdAsync(int id)
        {
            var getCMById = await _db.ContentMedias.Where(x => x.Id == id).ToListAsync();

            return getCMById;
        }

        public async Task<ContentMedia> GetContentMediaByIdAsync(int id)
        {
            return await _db.ContentMedias.FindAsync(id);
        }

        public async Task<List<ContentMedia>> GetFeaturedContentMediaAysnc()
        {
            var getFeaturedCM =await _db.ContentMedias.Where(x => x.Content.IsFeatured).ToListAsync();
            return getFeaturedCM;
        }

        public async Task<List<ContentMedia>> GetLatestContentMediaAsync(int count)
        {
            var getLatestCM = await _db.ContentMedias.OrderByDescending(cm => cm.Content.PublishedDate).Take(count).ToListAsync();
            return getLatestCM;
        }
    }
}
