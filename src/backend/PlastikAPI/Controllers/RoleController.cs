using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BLL.AbstractServices;
using BLL.Dtos.RoleDtos;
using Microsoft.AspNetCore.Authorization;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PlastikAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleController(IRoleService roleService, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleService = roleService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Add Role
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRoleDto dto)
        {
            try
            {
                // Role Name üzerinden varlığını kontrol edin
                var existingRole = await _roleService.GetRoleByNameAsync(dto.RoleName);
                if (existingRole != null)
                {
                    return BadRequest(new { error = "Bu rol zaten mevcut." });
                }

                await _roleService.AddRoleAsync(dto);
                return Ok(new { message = "Rol başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRoleAsync([FromBody] DeleteRoleDto dto)
        {
            try
            {
                var existingRole = await _roleService.GetRoleById(dto.Id);
                if (existingRole == null)
                {
                    return NotFound(new { error = "Silinmek istenen rol bulunamadı!" });
                }

                await _roleService.DeleteRoleAsync(dto);
                return Ok(new { message = "Rol başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Get All Roles
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Get Role by Id
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            try
            {
                var role = await _roleService.GetRoleById(id);

                if (role == null)
                {
                    return NotFound(new { error = "Role not found." });
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        // Update Role
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] UpdateRoleDto dto)
        {
            try
            {
                // Gelen DTO'daki RoleName ve Description doğrulamasını kontrol et
                if (string.IsNullOrEmpty(dto.RoleName) && string.IsNullOrEmpty(dto.Description))
                {
                    return BadRequest(new { error = "Güncelleme için en az bir alan belirtilmelidir." });
                }

                // Role güncelleme işlemini çağır
                await _roleService.UpdateRoleAsync(id, dto);

                // Başarılı bir güncelleme mesajı döndür
                return Ok(new { message = "Rol başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                // Hata durumunda detaylı mesaj döndür
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            try
            {
                var roleExists = await _roleService.GetRoleById(dto.RoleId);
                if (roleExists == null)
                {
                    return NotFound(new { error = "Belirtilen rol bulunamadı." });
                }

                await _roleService.AssignRoleAsync(dto.UserId, dto.RoleId);
                return Ok(new { message = "Rol başarıyla atandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UnassignRole")]
        public async Task<IActionResult> UnassignRole([FromBody] UnassignRoleDto dto)
        {
            try
            {
                var roleExists = await _roleService.GetRoleById(dto.RoleId);
                if (roleExists == null)
                {
                    return NotFound(new { error = "Belirtilen rol bulunamadı." });
                }

                await _roleService.UnassignRoleAsync(dto.UserId, dto.RoleId);
                return Ok(new { message = "Rol başarıyla kaldırıldı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
            
        [Authorize(Roles = "Admin")]
        [HttpGet("get-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            var userRolesIntersection = roles.IntersectBy(userRoles, r => r.Name).Select(r => new RoleDto
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();

            return Ok(userRolesIntersection);
        }

    }
}