using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ContentDto
{
    public class ContentDeleteDto
    {
        [Required]
        public int Id { get; set; }
        public List<IFormFile>? MediaFiles { get; set; }

    }
}
