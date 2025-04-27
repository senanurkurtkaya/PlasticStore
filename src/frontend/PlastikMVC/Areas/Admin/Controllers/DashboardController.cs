using Microsoft.AspNetCore.Mvc;
using PlastikMVC.Filters;

namespace PlastikMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PlastikAuth(Roles = "Admin,İçerik Yöneticisi,Müşteri Hizmetleri")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
