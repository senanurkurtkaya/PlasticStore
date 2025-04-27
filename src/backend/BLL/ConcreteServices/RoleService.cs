using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos.RoleDtos;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        

        public RoleService(RoleManager<Role> roleManager, IMapper mapper, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            
        }
        public async Task AddRoleAsync(CreateRoleDto createRoleDto)
        {
            if (string.IsNullOrEmpty(createRoleDto.RoleName))
                throw new ArgumentException("Rol ismi boş olamaz.");

            var roleExists = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);
            if (roleExists)
                throw new InvalidOperationException("Bu rol zaten mevcut.");

            var newRole = new Role
            {
                Name = createRoleDto.RoleName,
                NormalizedName = createRoleDto.RoleName.ToUpper(),
                Description = createRoleDto.Description // Açıklamayı atıyoruz
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
                throw new Exception("Rol oluşturulamadı: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }



        public async Task DeleteRoleAsync(DeleteRoleDto DeleteRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(DeleteRoleDto.Id);
            if (role == null)
            {
                throw new Exception("Böyle bir rol bulunamadı.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                throw new Exception("Bu rol hala kullanıcılarla ilişkilendirilmiş durumda, silemezsiniz.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception("Rol silinemedi: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<List<GetAllRolesDto>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => new GetAllRolesDto
            {
                Id = r.Id,
                RoleName = r.Name
            }).ToList();
        }

        public async Task<GetRoleByIdDto> GetRoleById(string id)
        {
            var roleGetId = await _roleManager.FindByIdAsync(id);
            if (roleGetId == null)
                throw new Exception("Rol bulunamadı.");

            return _mapper.Map<GetRoleByIdDto>(roleGetId);
        }


        public async Task UpdateRoleAsync(string id, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                throw new Exception("Güncellenecek rol bulunamadı.");

            // Rol ismini kontrol et ve güncelle
            if (!string.Equals(role.Name, updateRoleDto.RoleName, StringComparison.OrdinalIgnoreCase))
            {
                var roleExists = await _roleManager.RoleExistsAsync(updateRoleDto.RoleName);
                if (roleExists)
                    throw new Exception("Bu isimde başka bir rol zaten mevcut.");
                role.Name = updateRoleDto.RoleName;
                role.NormalizedName = updateRoleDto.RoleName.ToUpper();
            }

            // Açıklamayı güncelle
            if (!string.IsNullOrEmpty(updateRoleDto.Description))
            {
                role.Description = updateRoleDto.Description;
            }

            // Güncellenmiş rolü kaydet
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
                throw new Exception("Rol güncellenemedi: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }


        public async Task AssignRoleAsync(string userId, string roleId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
                {
                    throw new ArgumentException("UserId veya RoleId boş olamaz.");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null)
                {
                    throw new Exception("Rol bulunamadı.");
                }

                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (isInRole)
                {
                    throw new Exception("Kullanıcı zaten bu role atanmış.");
                }

                // Kullanıcıya rol ekle
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new Exception("Rol atanamadı.");
                }

                // Kullanıcının güncel rollerini al ve RoleName sütununu güncelle
                var userRoles = await _userManager.GetRolesAsync(user);
                user.RoleName = string.Join(",", userRoles);

                // User tablosundaki değişikliği kaydet
                await _userManager.UpdateAsync(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        public async Task UnassignRoleAsync(string userId, string roleId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
                {
                    throw new ArgumentException("UserId veya RoleId boş olamaz.");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("Kullanıcı bulunamadı.");
                }

                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null)
                {
                    throw new Exception("Rol bulunamadı.");
                }

                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (!isInRole)
                {
                    throw new Exception("Kullanıcı bu role atanmış değil.");
                }

                // Rolü kaldır
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new Exception("Rol kaldırılamadı.");
                }

                // Kullanıcının güncel rollerini al ve RoleName sütununu güncelle
                var remainingRoles = await _userManager.GetRolesAsync(user);

                if (remainingRoles.Count == 0)
                {
                    // Eğer kullanıcıya ait hiçbir rol kalmazsa "User" rolünü ata
                    await _userManager.AddToRoleAsync(user, "User");
                    user.RoleName = "User";
                }
                else
                {
                    user.RoleName = string.Join(",", remainingRoles);
                }

                // User tablosundaki değişikliği kaydet
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return null;

            return new RoleDto
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description
            };
        }



        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

    }
}
