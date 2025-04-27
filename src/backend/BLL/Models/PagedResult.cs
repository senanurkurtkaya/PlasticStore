using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>(); // Sayfalama sonucu içerikler
        public int TotalCount { get; set; } // Toplam içerik sayısı
        public int Page { get; set; } // Mevcut sayfa
        public int PageSize { get; set; } // Sayfa başına içerik sayısı
        public bool HasNextPage => Page * PageSize < TotalCount; // Sonraki sayfa var mı?
        public bool HasPreviousPage => Page > 1; // Önceki sayfa var mı?
    }
}
