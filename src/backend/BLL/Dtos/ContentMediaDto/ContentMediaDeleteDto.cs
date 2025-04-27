using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ContentMediaDto
{
    public class ContentMediaDeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}
