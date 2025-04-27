using BLL.Dtos.CategoryDto;
using BLL.Dtos.FaqCategoryDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IFaqCategoryService
    {
        Task<IEnumerable<FaqCategoryDto>> GetAllCategoriesAsync();
        Task<FaqCategoryWithQuestionsDto> GetCategoryWithFaqsAsync(int categoryId);
        Task AddCategoryAsync(CreateCategoryFaqDto categoryDto);
        Task UpdateCategoryAsync(UpdateCategoryFaqDto categoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
