using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.RoleDtos
{
    public class CreateRoleDto
    {
        

        [Required(ErrorMessage = "Role adı gereklidir.")]
        [StringLength(50, ErrorMessage = "Role adı en fazla 50 karakter olabilir.")]
        public string RoleName { get; set; }

        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string? Description { get; set; } 
    }
}
