using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.RoleDtos
{
    public class GetRoleByIdDto
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }


    }
}
