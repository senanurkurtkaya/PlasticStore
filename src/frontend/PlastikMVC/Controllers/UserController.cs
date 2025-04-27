using Azure;
using Microsoft.AspNetCore.Mvc;
using PlastikMVC.AllDtos.UserDtos;
using PlastikMVC.HttpClientService.HttpClientForRoles;
using System.Text;
using System.Text.Json;

namespace PlastikMVC.Controllers
{
    // UserController CREATED//ONR
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserHttpClientServices _userHttpClientServices;
        private readonly string _baseUrl;
        public UserController(IHttpClientFactory httpClientFactory, UserHttpClientServices userHttpClientServices,IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _userHttpClientServices = userHttpClientServices ?? throw new ArgumentNullException(nameof(userHttpClientServices));
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        public async Task<IActionResult> Index()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Kullanıcı kaydını gerçekleştirmek için UserHttpClientServices sınıfını çağırıyoruz.
                var isRegistered = await _userHttpClientServices.RegisterUser(model);

                if (isRegistered)
                {
                    // Kayıt başarılıysa Login sayfasına yönlendir.
                    return RedirectToAction("Login");
                }

                // API başarısız yanıt dönerse hata mesajı ekle.
                ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız oldu. Lütfen tekrar deneyin.");
            }
            catch (Exception ex)
            {
                // İstisna oluşursa detaylı hata mesajı ekle.
                ModelState.AddModelError(string.Empty, $"Hata: {ex.Message}");
            }

            // Hata durumunda aynı sayfayı döndür.
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginModel userLoginModel)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(userLoginModel),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync($"{_baseUrl}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                // Token veya kullanıcı bilgilerini işleyebilirsiniz
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                // string id'yi Guid'e dönüştürme
                if (!Guid.TryParse(id, out var guidId))
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı ID'si.");
                    return RedirectToAction("Index");
                }

                var user = await _userHttpClientServices.GetUserById(guidId);
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }


        // POST:UserController/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var isUpdated = await _userHttpClientServices.UpdateUser(model);
                if (isUpdated)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Kullanıcı güncellenemedi.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Hata: {ex.Message}");
            }

            return View(model);
        }

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var user = await _userHttpClientServices.GetUserById(id);
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bilgisi alınırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteUserDto deleteUserDto)
        {
            try
            {
                var isDeleted = await _userHttpClientServices.DeleteUser(deleteUserDto.Id);
                if (isDeleted)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Kullanıcı silinemedi.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Hata: {ex.Message}");
            }

            return View(deleteUserDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Session temizleme
            Response.Cookies.Delete(".AspNetCore.Cookies"); // Cookie temizleme
            return RedirectToAction("Login");
        }

    }
}
