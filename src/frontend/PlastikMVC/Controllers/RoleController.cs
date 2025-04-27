using Microsoft.AspNetCore.Mvc;
using PlastikMVC.AllDtos.RoleDtos;
using PlastikMVC.HttpClientService.HttpClientForRoles;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlastikMVC.Models;
using Microsoft.AspNetCore.Authorization;
using PlastikMVC.Filters;

namespace PlastikMVC.Controllers
{

    public class RoleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RoleHttpClientServices _roleHttpClientServices;

        public RoleController(IHttpClientFactory httpClientFactory, RoleHttpClientServices roleHttpClientServices)
        {
            _httpClientFactory = httpClientFactory;
            _roleHttpClientServices = roleHttpClientServices;
        }

        // Roller listesi (Index)
        public async Task<IActionResult> Index()
        {
            try
            {
                var roles = await _roleHttpClientServices.GetAllRolesAsync();
                return View(roles);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Roller yüklenirken bir hata oluştu: {ex.Message}";
                return View(new List<GetAllRolesModel>());
            }
        }

        // Yeni rol oluşturma formu (Get)
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        // Yeni rol oluşturma işlemi 
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "Account");
                }

                await _roleHttpClientServices.CreateRoleAsync(model, token);
                TempData["SuccessMessage"] = "Rol başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Rol oluşturulamadı: {ex.Message}");
                return View(model);
            }
        }

        // Rol atama formu (Get)
        [HttpGet("assign-role/{userId}")]
        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId)
        {
            try
            {
                var roles = await _roleHttpClientServices.GetAllRolesAsync();
                var model = new AssignRoleViewModel
                {
                    UserId = userId,
                    Roles = roles.Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.RoleName
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Roller yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index", "User");
            }
        }


        // Rol atama işlemi (Post)
        [HttpPost("assign-role/{userId}")]
        [PlastikAuth(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(AssignRolePostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // AuthToken kontrolü
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "Account");
                }

                // Rol atama isteği
                await _roleHttpClientServices.AssignRoleAsync(model.UserId, model.RoleId, token);
                TempData["SuccessMessage"] = "Rol başarıyla atandı.";
                return RedirectToAction("UserList", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Rol atanamadı: {ex.Message}");
                return View(model);
            }
        }


        // Rol geri alma formu (Get)
        [HttpGet("unassign-role/{userId}")]
        public async Task<IActionResult> UnassignRole(string userId)
        {
            try
            {
                var roles = await _roleHttpClientServices.GetUserRolesAsync(userId);
                var model = new UnassignRoleViewModel
                {
                    UserId = userId,
                    Roles = roles.Select(r => new SelectListItem
                    {
                        Value = r.Id,
                        Text = r.RoleName
                    }).ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kullanıcı rolleri yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index", "User");
            }
        }


        // Rol geri alma işlemi 
        [HttpPost("unassign-role/{userId}")]
        public async Task<IActionResult> UnassignRole(UnassignRolePostViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // AuthToken kontrolü
                var token = HttpContext.Session.GetString("AuthToken");
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "Account");
                }

                // Rol kaldırma isteği
                await _roleHttpClientServices.UnassignRoleAsync(model.UserId, model.RoleId, token);
                TempData["SuccessMessage"] = "Rol başarıyla kaldırıldı.";
                return RedirectToAction("UserList", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Rol kaldırma işlemi başarısız: {ex.Message}");
                return View(model);
            }
        }


    }
}
