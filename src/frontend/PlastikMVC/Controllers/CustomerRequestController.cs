using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlastikMVC.HttpClientService.HttpClientForCustomerRequests;
using Newtonsoft.Json;
using PlastikMVC.Client.Models.Request.CustomerRequest;
using System.Net.Http.Headers;
using System.Text;
using PlastikMVC.Models.Category;
using PlastikMVC.Dtos.CategoryDto;
using PlastikMVC.Models;
using PlastikMVC.Filters;

namespace PlastikMVC.Controllers
{

    public class CustomerRequestController : Controller
    {
        private readonly HttpClientForCustomerRequests _clientForCustomerRequests;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;
        public CustomerRequestController(HttpClientForCustomerRequests clientForCustomerRequests, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _clientForCustomerRequests = clientForCustomerRequests;
            _httpClientFactory = httpClientFactory;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        public ActionResult Index()
        {
            //var getAllRequests = _clientForCustomerRequests.get
            return View();
        }

        // GET: CustomerRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerRequestController/Create
        [HttpGet]
        [PlastikAuth(Roles = "Admin,User")]
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("AuthToken");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //  API'den kategorileri çekiyoruz
            var response = await client.GetAsync($"{_baseUrl}/Categories");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kategoriler yüklenemedi.";
                return View(new CreateCustomerRequestViewModel());
            }

            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(content);

            ViewBag.Categories = categories; // ViewBag ile kategorileri gönderiyoruz
            return View(new CreateCustomerRequestViewModel());
        }

        [HttpPost]
        [PlastikAuth(Roles = "Admin,User")]
        public async Task<IActionResult> Create(CreateCustomerRequestViewModel model)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = HttpContext.Request.Cookies["AuthToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var jsonContent = JsonConvert.SerializeObject(model);
                Console.WriteLine("[DEBUG] API'ye giden veri: " + jsonContent);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_baseUrl}/customerrequest/CreateCustomerRequest", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Talep başarıyla oluşturuldu.";
                    return Redirect("/Admin/CustomerService");
                }
                else
                {
                    string errorMessage = string.Empty;
                    try
                    {
                        var error = await response.Content.ReadFromJsonAsync<ErrorModel>();
                        errorMessage = error.Message;
                    }
                    catch (Exception)
                    {
                        // ignore
                    }

                    TempData["ErrorMessage"] = string.IsNullOrEmpty(errorMessage) ? "Talep oluşturulamadı." : errorMessage;
                    return RedirectToAction("Create");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                return RedirectToAction("Create");
            }

            return View(model);
        }



        // POST: CustomerRequestController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CustomerRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateCustomerRequestViewModel requestModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(requestModel);
        //    }

        //    try
        //    {
        //        var client = _httpClientFactory.CreateClient();
        //        var token = HttpContext.Session.GetString("AuthToken");
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync($"{_baseUrl}/customerrequest", content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            TempData["ErrorMessage"] = "İade talebi oluşturulamadı!";
        //            return View(requestModel);
        //        }

        //        TempData["SuccessMessage"] = "İade talebiniz başarıyla oluşturuldu.";
        //        return RedirectToAction("Index", "CustomerService");  // Kullanıcı iade taleplerini listeleyen sayfaya yönlendirilir
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Beklenmeyen bir hata oluştu: {ex.Message}";
        //        return View(requestModel);
        //    }
        //}
    }
}
