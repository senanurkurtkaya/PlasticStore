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
    public class ContentRepository : IContentRepository
    {
        private readonly AppDbContext _db;

        public ContentRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<Content>> GetAllContentsAsync()
        {
            return await _db.Contents.Include(cm => cm.Medias).ToListAsync();
        }
    }
}
