using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PlastikMVC.AllDtos.UserDtos;
using PlastikMVC.Client;
using PlastikMVC.HttpClientService.HttpClientForRoles;
using PlastikMVC.Models;

namespace PlastikMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserHttpClientServices _userHttpClientServices;
        private readonly ProductClient _productClient;

        public HomeController(ILogger<HomeController> logger, UserHttpClientServices userHttpClientServices, ProductClient productClient)
        {
            _logger = logger;
            _userHttpClientServices = userHttpClientServices;
            _productClient = productClient;
        }


        public async Task<IActionResult> Index()
        {
            var top20Products = await _productClient.GetTop20Products();

            ViewBag.Top20Products = top20Products;

            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult About()
        {
            var teamMembers = new List<TeamMember>
    {
        new TeamMember
        {
            Name = "Muhammed Taha KARAKOÇ",
            Title = "CEO",
            ImagePath = "/images/FullSizeRender.jpg",
            TwitterUrl = "https://x.com/mtk1997",
            LinkedInUrl = "https://www.linkedin.com/in/muhammed-taha-karakoç/",
            InstagramUrl = "https://www.instagram.com/taha.karakoc/"
        },
        new TeamMember
        {
            Name = "Sena KURTKAYA",
            Title = "Teknoloji Lideri",
            ImagePath = "/images/sena.jpg",
            TwitterUrl = "",
            LinkedInUrl = "https://www.linkedin.com/in/sena-kurtkaya-289638248/",
            InstagramUrl = "https://www.instagram.com/senanurkurtkaya/"
        },
        new TeamMember
        {
            Name = "Onur TOSUN",
            Title = "Pazarlama Uzmaný",
            ImagePath = "/images/onurp.jpg",
            TwitterUrl = "",
            LinkedInUrl = "https://www.linkedin.com/in/onur-tosun-53b6921b0/",
            InstagramUrl = "https://www.instagram.com/onurlu.tosun/"
        }
    };

            return View(teamMembers);
        }
        public IActionResult Privacy()
        {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
