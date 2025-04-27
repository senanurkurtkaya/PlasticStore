using Microsoft.AspNetCore.Mvc;

namespace PlastikMVC.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
