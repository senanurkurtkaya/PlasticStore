using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using PlastikMVC.AllDtos.UserDtos;
using PlastikMVC.HttpClientService.HttpClientForRoles;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PlastikMVC.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace PlastikMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserHttpClientServices _userHttpClientServices;
        private readonly ILogger<AccountController> _logger;
        private readonly string _baseUrl;
        

        public AccountController(
            IHttpClientFactory httpClientFactory,
            UserHttpClientServices userHttpClientServices,
            ILogger<AccountController> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _userHttpClientServices = userHttpClientServices ?? throw new ArgumentNullException(nameof(userHttpClientServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        // Kullanıcı Girişi (GET)
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Kullanıcı Girişi (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginModel);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(userLoginModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_baseUrl}/user/login", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Giriş başarısız: {error}");
                    return View(userLoginModel);
                }

                var responseData = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API'den gelen yanıt: {responseData}");

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseData);

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz yanıt alındı.");
                    return View(userLoginModel);
                }

                // Session içine kullanıcı bilgilerini ve rolleri sakla
                HttpContext.Session.SetString("AuthToken", tokenResponse.Token);
                HttpContext.Session.SetString("UserRoles", JsonConvert.SerializeObject(tokenResponse.User.Roles));
                HttpContext.Session.SetString("UserId", tokenResponse.User.Id); 

            HttpContext.Response.Cookies.Append("AuthToken", tokenResponse.Token);

            //HttpContext.Response.Cookies.Append("AuthToken", tokenResponse.Token);

            if (tokenResponse.User != null)
            {
                HttpContext.Session.SetString("UserId", tokenResponse.User.Id);
                HttpContext.Session.SetString("UserName", tokenResponse.User.UserName);
                HttpContext.Session.SetString("UserRoles", JsonConvert.SerializeObject(tokenResponse.User.Roles));

                HttpContext.Response.Cookies.Append("UserRoles", JsonConvert.SerializeObject(tokenResponse.User.Roles));
            }
           
            if (tokenResponse.User?.Roles != null && tokenResponse.User.Roles.Contains("Admin"))
            {
                //return RedirectToAction("AdminDashboard", "Admin");
                return Redirect("/");
            }
            else if (tokenResponse.User?.Roles != null && tokenResponse.User.Roles.Contains("User"))
            {
                return RedirectToAction("Index", "Home");
            }

                ModelState.AddModelError(string.Empty, "Yetkisiz erişim. Lütfen tekrar giriş yapın.");
                return View(userLoginModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login sırasında bir hata oluştu: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Lütfen tekrar deneyin.");
                return View(userLoginModel);
            }
        }




        // Kullanıcı Çıkışı

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("UserRoles");
            return RedirectToAction("Login");
        }

        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalı model doğrulama durumunda sayfayı tekrar göster
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                // API'ye kayıt isteği gönder
                var response = await client.PostAsync($"{_baseUrl}/user/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login"); // Başarılı kayıt durumunda Login sayfasına yönlendir
                }

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Kayıt başarısız oldu: {error}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
            }

            return View(model);
        }

        // Profil Görüntüleme
        
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Kullanıcının token'ını Session'dan al
            var token = HttpContext.Session.GetString("AuthToken");

            // Token yoksa kullanıcıyı "Login" sayfasına yönlendir
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("Login");
            }

            // HttpClient ayarlarını yapılandır
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'ye istek gönder ve yanıtı kontrol et
            var response = await client.GetAsync($"{_baseUrl}/user/profile");

            if (!response.IsSuccessStatusCode)
            {
                // Token geçersiz veya başka bir hata durumunda ana sayfaya yönlendir
                TempData["ErrorMessage"] = "Profil bilgilerine erişilemedi.";
                return RedirectToAction("Index", "Home");
            }

            // API'den gelen veriyi çözümle ve profile modeline ata
            var content = await response.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<UserProfileModel>(content);

            // Görünümü dön
            return View(profile);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UpdateProfileModel model)
        {
           
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            // Model doğrulama
            if (!ModelState.IsValid)
            {
                return View(model); // Hatalı model durumunda formu tekrar göster
            }

            try
            {
                // JSON içeriği oluştur
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(model),
                    Encoding.UTF8,
                    "application/json"
                );

                // API isteği
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsync($"{_baseUrl}/user/profile/{model.Id}", jsonContent);

                // API'den hata dönerse, hata mesajını kullanıcıya göster
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Profil güncellenemedi: {errorMessage}");
                    return View(model);
                }

                // Başarılı bir şekilde güncelleme yapılmışsa kullanıcıya bildirin
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
                // Beklenmeyen bir hata durumunda kullanıcıyı bilgilendirin
                ModelState.AddModelError(string.Empty, $"Bir hata oluştu: {ex.Message}");
                return View(model);
            }
        }


        // Profil Düzenleme (GET)
        
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_baseUrl}/user/profile");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Profile", "Account");
            }

            var content = await response.Content.ReadAsStringAsync();
            var userProfile = JsonConvert.DeserializeObject<UserProfileModel>(content);

            var updateProfileModel = new UpdateProfileModel
            {
                Id = userProfile.Id, 
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                UserName = userProfile.UserName
            };

            return View(updateProfileModel);
        }



        // Şifre Değiştirme (GET)
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }


        // Şifre Değiştirme (POST)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Oturum süreniz dolmuş. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            var changePasswordDto = new ChangePasswordDto
            {
                Id = userId, // Kullanıcı ID'sini ekle
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.ConfirmNewPassword
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(changePasswordDto),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Session.GetString("AuthToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync($"{_baseUrl}/user/change-password", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API hatası: {response.StatusCode} - {errorMessage}");
                    ModelState.AddModelError("", $"Şifre değiştirilemedi: {errorMessage}");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre değiştirme sırasında bir hata oluştu: {ex.Message}");
                ModelState.AddModelError("", "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.");
                return View(model);
            }
        }

        public IActionResult AccessDenied()
        {
            ViewBag.ErrorMessage = "Erişim Yetkiniz Bulunmamaktadır.";
            return View();
        }


        
        [HttpGet]
        public IActionResult AdminDashboard()
        {
            var roles = JsonConvert.DeserializeObject<List<string>>(HttpContext.Request.Cookies["UserRoles"]);

            if (roles == null || !roles.Contains("Admin"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                    return RedirectToAction("Login");
                }

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"{_baseUrl}/user/all-users");

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Kullanıcı listesi alınamadı. Detay: {errorDetails}";
                    return RedirectToAction("AdminDashboard");
                }

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDto>>(content);

                var userRoles = JsonConvert.DeserializeObject<List<string>>(HttpContext.Session.GetString("UserRoles"));
                if (!userRoles.Contains("Admin"))
                {
                    TempData["ErrorMessage"] = "Bu sayfaya erişim yetkiniz yok.";
                    return RedirectToAction("AccessDenied", "Account");
                }

                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }



        
        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            try
            {
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                    return RedirectToAction("Login");
                }

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync($"{_baseUrl}/user/make-admin/{userId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Admin rolü atanamadı. Detay: {errorDetails}";
                }
                else
                {
                    TempData["SuccessMessage"] = "Kullanıcıya admin rolü başarıyla verildi.";
                }

                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("UserList");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> RevokeAdmin(string userId)
        {
            try
            {
                // Admin yetkisini kaldırma işlemi
                var token = HttpContext.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                    return RedirectToAction("Login");
                }

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync($"{_baseUrl}/user/revoke-admin/{userId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Admin yetkisi kaldırılmadı. Detay: {errorDetails}";
                }
                else
                {
                    TempData["SuccessMessage"] = "Admin yetkisi başarıyla kaldırıldı.";

                    // Kullanıcı oturumundaki roller güncelleniyor
                    var updatedRolesResponse = await client.GetAsync($"{_baseUrl}/user/get-roles");
                    if (updatedRolesResponse.IsSuccessStatusCode)
                    {
                        var updatedRolesContent = await updatedRolesResponse.Content.ReadAsStringAsync();
                        var updatedRoles = JsonConvert.DeserializeObject<List<string>>(updatedRolesContent);

                        HttpContext.Session.SetString("UserRoles", JsonConvert.SerializeObject(updatedRoles));
                    }
                }

                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("UserList");
            }
        }


        // Kullanıcı Dashboard
        
        [HttpGet]
        public IActionResult UserDashboard()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        
        public IActionResult ContentManagerDashboard()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLastLogin([FromBody] UpdateLastLoginDto updateLastLoginDto)
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(updateLastLoginDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_baseUrl}/User/UpdateLastLogin", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Empty request has taken from API!");
            }
       
        

            return NoContent();
        }

        public IActionResult CustomerServiceDashboard()
        {
            return View();
        }
    }
}
