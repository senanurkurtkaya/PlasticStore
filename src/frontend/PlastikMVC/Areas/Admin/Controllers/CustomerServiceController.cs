using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlastikMVC.Client.Models.Request.CustomerRequest;
using PlastikMVC.Filters;
using System.Net.Http.Headers;
using System.Net.Http;
using PlastikMVC.HttpClientService.HttpClientForCustomerRequests;
using System.Text;

namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PlastikAuth(Roles = "Admin")]
    public class CustomerServiceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientForCustomerRequests _customerRequestService;
        private readonly string _baseUrl;

        public CustomerServiceController(IHttpClientFactory httpClientFactory, IConfiguration configuration, HttpClientForCustomerRequests httpClientForCustomerRequests)
        {
            _httpClientFactory = httpClientFactory;
            _customerRequestService = httpClientForCustomerRequests;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string filter = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AuthToken"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync($"{_baseUrl}/customerrequest?filter={filter}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Müşteri talepleri alınırken bir hata oluştu.";
                    return View(new List<CustomerRequestViewModel>());
                }

                var content = await response.Content.ReadAsStringAsync();
                var requests = JsonConvert.DeserializeObject<List<CustomerRequestViewModel>>(content);

                return View(requests);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Beklenmeyen bir hata oluştu: {ex.Message}";
                return View(new List<CustomerRequestViewModel>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int requestId, string newStatus)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AuthToken"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var updateDto = new
                {
                    Status = newStatus
                };

                var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_baseUrl}/customerrequest/{requestId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Talep durumu güncellenemedi.";
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "Talep durumu başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Beklenmeyen bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
