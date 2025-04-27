using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PlastikMVC.Controllers
{
    //[Route("admin")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [HttpGet("admindashboard")]
        public IActionResult AdminDashboard()
        {
            return View();
        }


        [HttpGet("usermanagement")]
        public IActionResult UserManagement()
        {
            return View();
        }
    }

}
