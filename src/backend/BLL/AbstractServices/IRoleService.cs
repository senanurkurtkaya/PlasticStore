using BLL.Dtos.RoleDtos;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IRoleService
    {
        Task AddRoleAsync(CreateRoleDto CreateRoleDto);
        Task DeleteRoleAsync(DeleteRoleDto DeleteRoleDto);
        Task UpdateRoleAsync(string id,UpdateRoleDto UpdateRoleDto);
        Task<List<GetAllRolesDto>> GetAllRoles();
        Task<GetRoleByIdDto> GetRoleById(string id);
        Task AssignRoleAsync(string userId, string roleId);
        Task UnassignRoleAsync(string userId, string roleId);
        Task<RoleDto> GetRoleByNameAsync(string roleName);
    }
}
