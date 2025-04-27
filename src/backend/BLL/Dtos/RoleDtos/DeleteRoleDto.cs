using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.RoleDtos
{
    public class DeleteRoleDto
    {
        [Required(ErrorMessage = "Role ID'si gereklidir.")]
        public string Id { get; set; }

        

    }
}
