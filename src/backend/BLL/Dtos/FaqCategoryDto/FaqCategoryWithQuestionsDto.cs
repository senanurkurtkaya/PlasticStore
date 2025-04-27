using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.FaqCategoryDto
{
    public class FaqCategoryWithQuestionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FaqCategoryDto> Questions { get; set; } = new List<FaqCategoryDto>();
    }
}
