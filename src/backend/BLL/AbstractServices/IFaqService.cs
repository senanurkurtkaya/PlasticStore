using BLL.Dtos.FaqDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IFaqService
    {
        Task<IEnumerable<FaqDto>> GetAllFaqsAsync();

    
        Task<IEnumerable<FaqDto>> GetFaqsByCategoryAsync(int categoryId);

        Task<FaqDto> GetFaqByIdAsync(int id);

        
        Task AddFaqAsync(CreateFaqDto faqDto);

        
        Task UpdateFaqAsync(UpdateFaqDto faqDto);

    
        Task DeleteFaqAsync(int id);
    }
}
