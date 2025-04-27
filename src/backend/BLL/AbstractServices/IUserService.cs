using BLL.Dtos;
using BLL.Dtos.UserReportsDto;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        void UpdateProfile(int userId, UpdateProfileDto updateProfileDto);
        Task<List<ActiveUsersDto>> GetDailyActiveUsersAsync();
        Task<List<ActiveUsersDto>> GetWeeklyActiveUsersAsync();
        Task<List<ActiveUsersDto>> GetMonthlyActiveUsersAsync();        
        Task UpdateLastLoginDateAsync(UpdateLastLoginDto updateLastLoginDto);
        Task<UserReportDto> GetUserStatisticsWithDetailsAsync();
        Task<List<DetaileduserReportDto>> GetDetailedUserReportAsync();
    }
}
