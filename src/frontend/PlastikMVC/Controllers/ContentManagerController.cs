using Microsoft.AspNetCore.Mvc;

namespace PlastikMVC.Controllers
{

    public class ContentManagerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
