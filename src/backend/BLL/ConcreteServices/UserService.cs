using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos;
using BLL.Dtos.UserReportsDto;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.ConcreteServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return _userManager.Users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive
            }).AsEnumerable();
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            // Yeni kullanıcı oluşturma
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            // Kullanıcıyı güncelleme
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı" });
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<ProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
                return null;
            
            return _mapper.Map<ProfileDto>(user);
        }

        public void UpdateProfile(int userId, UpdateProfileDto updateProfileDto)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            // Güncelleme işlemi
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.UserName = updateProfileDto.UserName;
            user.Email = updateProfileDto.Email;

            _context.SaveChanges();
        }

        public async Task<List<ActiveUsersDto>> GetDailyActiveUsersAsync()
        {
            var startOfDay = DateTime.Now.Date;
            var endOfDay = startOfDay.AddDays(1);
            
            var dailyActiveusers = await _context.Users.Where(x => x.LastLoginDate >= startOfDay && x.LastLoginDate < endOfDay).Select(u=> new ActiveUsersDto
            {
                FullName = $"Ad: {u.FirstName} {u.LastName}",
                Email = u.Email,
                LastLoginDate = u.LastLoginDate,
            }).ToListAsync();            

            return dailyActiveusers;

        }

        public async Task<List<ActiveUsersDto>> GetWeeklyActiveUsersAsync()
        {
            var endWeek = DateTime.Now.Date;

            var startWeek = endWeek.AddDays(-7);

            var weeklyActiveUsers = await _context.Users.Where(u => u.LastLoginDate >= startWeek && u.LastLoginDate < endWeek).Select(u => new ActiveUsersDto
            {
                FullName = $"Ad: {u.FirstName} {u.LastName}",
                Email = u.Email,
                LastLoginDate = u.LastLoginDate,
            }).ToListAsync();           

            return weeklyActiveUsers;

        }

        public async Task<List<ActiveUsersDto>> GetMonthlyActiveUsersAsync()
        {
            var endMonth = DateTime.Now.Date;
            var startMonth = endMonth.AddDays(-30);

            var monthlyActiveUsers = await _context.Users.Where(u => u.LastLoginDate >= startMonth && u.LastLoginDate <= endMonth).Select(u=> new ActiveUsersDto
            {
                FullName = $"Ad: {u.FirstName} {u.LastName}",
                Email = u.Email,
                LastLoginDate = u.LastLoginDate,
            }).ToListAsync();

            return monthlyActiveUsers;

        }

        public async Task UpdateLastLoginDateAsync(UpdateLastLoginDto lastLoginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id==lastLoginDto.UserId);

            if (user == null)

            {
                throw new Exception("Kullanıcı bulunamadı!");
            }          

            if(user.LastLoginDate==null || (DateTime.Now - user.LastLoginDate.Value).TotalMinutes > 10)
                
            {
                    user.LastLoginDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                
            }      

        }

        public async Task<UserReportDto> GetUserStatisticsWithDetailsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await  _context.Users.CountAsync(a => a.IsActive);

            var inactiveUsers = await _context.Users.CountAsync(a=>!a.IsActive);

            var usersCreatedThisMonth = await _context.Users.Where(u => u.CreatedAt.Month == DateTime.Now.Month && u.CreatedAt.Year == DateTime.Now.Year).CountAsync();

            var adminUsers = await _context.Users.CountAsync(u=>u.IsAdmin==true);

            return new UserReportDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                UsersCreatedThisMonth = usersCreatedThisMonth,
                AdminUsers = adminUsers
            };
            
        }

        public async Task<List<DetaileduserReportDto>> GetDetailedUserReportAsync()
        {
            return await _context.Users.Include(inc=>inc.IsAdmin).Select(u => new DetaileduserReportDto
            {
               FullName = $"Adı :  {u.FirstName} Soyadı :  {u.LastName}",
               Email = u.Email,              
               CreatedAt = u.CreatedAt,
               IsActive = u.IsActive,
               LastLoginDate = u.LastLoginDate,

            }).ToListAsync();    
        }

        public Task UpdateLastLoginDateAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}