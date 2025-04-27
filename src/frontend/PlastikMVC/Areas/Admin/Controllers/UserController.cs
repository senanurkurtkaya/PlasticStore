using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Newtonsoft.Json;
using PlastikMVC.Models;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using PlastikMVC.AllDtos.UserDtos;

namespace PlastikMVC.Areas.Admin.Controllers
{
   
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)

        {

            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        }

        public IActionResult UserDashboard()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(UserRegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var client = _httpClientFactory.CreateClient();
        //    var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

        //    // Register endpointini güncelleyin
        //    var response = await client.PostAsync($"{_baseUrl}/user/register", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    ModelState.AddModelError(string.Empty, "Kayıt başarısız oldu. Lütfen tekrar deneyin.");
        //    return View(model);
        //}

        
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}
        // GET: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(UserLoginModel userLoginModel)
        //{
        //    //var client = _httpClientFactory.CreateClient();

        //    //var jsonContent = new StringContent(
        //    //    JsonSerializer.Serialize(userLoginModel),
        //    //    Encoding.UTF8,
        //    //    "application/json");

        //    //var response = await client.PostAsync("", jsonContent);

        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var responseData = await response.Content.ReadAsStringAsync();
        //    //    // Token veya kullanıcı bilgilerini işleyebilirsiniz
        //    //    return RedirectToAction("Index", "Home");
        //    //}

        //    //ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
        //    return View();

        //}

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            //var client = _httpClientFactory.CreateClient();
            //var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            //var response = await client.PostAsync($"{_baseUrl}", content);

            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
            return View();

        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {

            return RedirectToAction("Index");  //burada eksiklik vardı kimin bilmiyorum   -sena
        }
    }
}
