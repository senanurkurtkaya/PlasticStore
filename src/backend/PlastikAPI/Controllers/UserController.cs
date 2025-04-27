// PlastikAPI - UserController.cs
using BLL.AbstractServices;
using BLL.ConcreteServices;
using BLL.Dtos;
using BLL.Dtos.UserReportsDto;
using BLL.Validators;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlastikAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IUserService userService, RoleManager<Role> roleManager, ILogger<UserController> logger, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
            _roleManager = roleManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context;
        }

        // Login işlemi
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Eksik veya hatalı bilgi." });
            }

            // Kullanıcı girişini doğrula
            var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Kullanıcı adı veya şifre hatalı." });
            }

            // Kullanıcıyı bul
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return Unauthorized(new { Message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcının rollerini al
            var roles = await _userManager.GetRolesAsync(user);

            // Roller boşsa loglayın
            if (roles == null || !roles.Any())
            {
                Console.WriteLine("Kullanıcıya atanmış herhangi bir rol bulunamadı.");
            }
            else
            {
                Console.WriteLine("Kullanıcı Roller: " + string.Join(", ", roles));
            }

            // JWT Token için gizli anahtarı al
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JwtSettings:SecretKey yapılandırmada eksik.");
            }

            // JWT oluşturmak için gerekli ayarlar
            //var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim("UserId", user.Id),
        //    new Claim("Email", user.Email ?? "")
        //}),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        //    };

            // Kullanıcının rollerini token'a ekle
            //foreach (var role in roles)
            //{
            //    Console.WriteLine($"Token'a Eklenen Rol: {role}");
            //    tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim("Email", user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "User") // Kullanıcı rolü ekleyelim
            };

            foreach (var role in roles)
            {
                Console.WriteLine($"Token'a Eklenen Rol: {role}");
                claims.Add(new Claim(ClaimTypes.Role, role));

                //tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            // Token oluştur ve dön
            // var token = tokenHandler.CreateToken(tokenDescriptor);

            Console.WriteLine("JWT Token Başarıyla Oluşturuldu.");

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                User = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    Roles = roles
                }
            });
        }



        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı verisi." });
            }

            var roleName = userDto.IsAdmin ? "Admin" : "User";
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new Role
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                var roleCreationResult = await _roleManager.CreateAsync(role);
                if (!roleCreationResult.Succeeded)
                {
                    return BadRequest(new { Message = $"{roleName} rolü oluşturulamadı.", Errors = roleCreationResult.Errors });
                }
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userDto.UserName,
                Email = userDto.Email,
                NormalizedEmail = userDto.Email?.ToUpperInvariant(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                IsAdmin = userDto.IsAdmin,
                RoleName = roleName
            };

            try
            {
                var userCreationResult = await _userManager.CreateAsync(user, userDto.Password);
                if (!userCreationResult.Succeeded)
                {
                    return BadRequest(new { Message = "Kullanıcı oluşturulamadı.", Errors = userCreationResult.Errors });
                }

                var roleAssignResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleAssignResult.Succeeded)
                {
                    return BadRequest(new { Message = "Rol ataması başarısız oldu.", Errors = roleAssignResult.Errors });
                }

                return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Kullanıcı kaydı sırasında bir hata oluştu.", Error = ex.Message });
            }
        }



        [Authorize(Roles ="Admin")]
        // Kullanıcıyı sil
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Kullanıcı başarıyla silindi." });
        }

        // Kullanıcıyı güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { Message = "Geçersiz kullanıcı verisi." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcı bilgilerini güncelle
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Kullanıcı başarıyla güncellendi." });
        }

        [Authorize(Policy = "AllRoles")]
        // Kullanıcı profilini al
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue("UserId"); // Kullanıcı ID'sini al
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Kullanıcı doğrulanamadı." });
            }

            var user = await _userManager.FindByIdAsync(userId); // ID ile kullanıcıyı bul
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var profile = new ProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin
            };

            return Ok(profile);
        }

        // Tüm kullanıcıları listele
        //[Authorize(Policy = "MultiRolePolicy")]
        [AllowAnonymous]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin
            }).ToList();

            // Roller bilgilerini kullanıcılarla eşleştir
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = (await _userManager.GetRolesAsync(appUser)).ToList();
            }

            return Ok(users);
        }


        [Authorize(Policy = "MultiRolePolicy")]
        // Belirli bir kullanıcıyı getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                return BadRequest(new { Message = "Geçersiz ID formatı." });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var userDetails = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.IsAdmin
            };

            return Ok(userDetails);
        }

        [Authorize(Policy = "MultiRolePolicy")]
        [HttpGet("profile-by-username/{username}")]
        public async Task<IActionResult> GetProfileByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var profile = new ProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin
            };

            return Ok(profile);
        }

        // Profil güncelleme
        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] UpdateProfileDto updateProfileDto)
        {
            // Validasyon kontrolü
            var validator = new UpdateProfileValidator();
            var validationResult = validator.Validate(updateProfileDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }
            // Kullanıcı adının başka bir kullanıcı tarafından kullanılıp kullanılmadığını kontrol et
            var existingUser = await _userManager.FindByNameAsync(updateProfileDto.UserName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return BadRequest(new { Message = "Bu kullanıcı adı zaten kullanımda." });
            }

            // Güncellenebilir alanlar
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.UserName = updateProfileDto.UserName;
            user.Email = updateProfileDto.Email;

            // Şifre güncellemeyi burada kaldırabilirsiniz
            // user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updateProfileDto.Password);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Güncelleme işlemi başarısız.", Errors = result.Errors });
            }

            return Ok(new { Message = "Profil başarıyla güncellendi." });
        }


        // Şifre değiştirme
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (string.IsNullOrEmpty(changePasswordDto.Id) ||
                string.IsNullOrEmpty(changePasswordDto.CurrentPassword) ||
                string.IsNullOrEmpty(changePasswordDto.NewPassword))
            {
                return BadRequest(new { Message = "ID, mevcut şifre ve yeni şifre alanları zorunludur." });
            }

            var user = await _userManager.FindByIdAsync(changePasswordDto.Id);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Şifre değiştirme işlemi başarısız.", Errors = result.Errors });
            }

            return Ok(new { Message = "Şifre başarıyla değiştirildi." });
        }


        [Authorize(Policy = "MultiRolePolicy")]
        [HttpGet("GetDailyActiveUsers")]
        public async Task<IActionResult> GetDailyActiveUsersAsync()
        {
            try
            {
                var getDailyActiveUsers = await _userService.GetDailyActiveUsersAsync();
                return Ok(getDailyActiveUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "MultiRolePolicy")]
        [AllowAnonymous]
        [HttpGet("WeeklyActiveUsers")]
        public async Task<IActionResult> GetWeeklyActiveUsersAsync()
        {
            try
            {
                var getWeeklyActiveUsers = await _userService.GetWeeklyActiveUsersAsync();
                return Ok(getWeeklyActiveUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "MultiRolePolicy")]
        [AllowAnonymous]
        [HttpGet("MonthlyActiveUsers")]
        public async Task<IActionResult> GetMonthlyActiveUsersAsync()
        {
            try
            {
                var getMonthlyActiveUsersAsync = await _userService.GetMonthlyActiveUsersAsync();
                return Ok(getMonthlyActiveUsersAsync);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Policy = "MultiRolePolicy")]
        [HttpPut("UpdateLastLogin")]
        public async Task<IActionResult> UpdateLastLogin([FromBody] UpdateLastLoginDto updateLastLoginDto)
        {
            if (updateLastLoginDto == null || string.IsNullOrEmpty(updateLastLoginDto.UserId))
            {
                return BadRequest("Geçersiz kullanıcı bilgileri.");
            }

            try
            {
                await _userService.UpdateLastLoginDateAsync(updateLastLoginDto);
                return NoContent(); // Güncelleme başarılı, içerik döndürülmüyor
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-admin-dashboard")]
        public IActionResult GetAdminDashboard()
        {
            return Ok("Admin paneline hoş geldiniz!");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("make-admin/{userId}")]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            // Kullanıcıyı veritabanından al
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            // Admin rolünün mevcut olup olmadığını kontrol et
            var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExists)
            {
                return BadRequest("Admin rolü mevcut değil.");
            }

            // Kullanıcının mevcut rollerini al
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Eğer kullanıcıda "User" rolü varsa, sadece onu kaldır
            if (currentRoles.Contains("User"))
            {
                var removeUserRoleResult = await _userManager.RemoveFromRoleAsync(user, "User");
                if (!removeUserRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", removeUserRoleResult.Errors.Select(e => e.Description));
                    return BadRequest($"User rolü kaldırılamadı. Hata: {errors}");
                }
            }

            // Kullanıcıya Admin rolünü ekle
            var addAdminRole = await _userManager.AddToRoleAsync(user, "Admin");
            if (!addAdminRole.Succeeded)
            {
                var errors = string.Join(", ", addAdminRole.Errors.Select(e => e.Description));
                return BadRequest($"Admin yetkisi atanamadı. Hata: {errors}");
            }

            // Kullanıcının IsAdmin özelliğini güncelle
            user.IsAdmin = true;
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                var errors = string.Join(", ", updateUserResult.Errors.Select(e => e.Description));
                return BadRequest($"Kullanıcı bilgileri güncellenemedi. Hata: {errors}");
            }

            return Ok("Kullanıcıya Admin yetkisi verildi ve sadece 'User' rolü kaldırıldı, diğer roller korundu.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("revoke-admin/{userId}")]
        public async Task<IActionResult> RevokeAdmin(string userId)
        {
            // Kullanıcıyı veritabanından al
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            // Admin rolünü kaldır
            var removeAdminRoleResult = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (!removeAdminRoleResult.Succeeded)
            {
                var errors = string.Join(", ", removeAdminRoleResult.Errors.Select(e => e.Description));
                return BadRequest($"Admin yetkisi kaldırılamadı. Hata: {errors}");
            }

            // Kullanıcının rollerini tekrar al (Admin kaldırıldıktan sonra)
            var updatedRoles = await _userManager.GetRolesAsync(user);

            // Eğer kullanıcının başka bir rolü yoksa, ona User rolünü ekle 
            if (updatedRoles.Count == 0)
            {
                var addUserRoleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!addUserRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addUserRoleResult.Errors.Select(e => e.Description));
                    return BadRequest($"User rolü atanamadı. Hata: {errors}");
                }
            }

            // Kullanıcının IsAdmin özelliğini güncelle
            user.IsAdmin = false;
            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                var errors = string.Join(", ", updateUserResult.Errors.Select(e => e.Description));
                return BadRequest($"Kullanıcı bilgileri güncellenemedi. Hata: {errors}");
            }

            return Ok("Admin rolü kaldırıldı, diğer roller korundu. Eğer kullanıcıda başka rol yoksa 'User' rolü eklendi.");
        }



    }
}
