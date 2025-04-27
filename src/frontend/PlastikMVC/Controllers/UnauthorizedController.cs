using Microsoft.AspNetCore.Mvc;

namespace PlastikMVC.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
