using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PlastikMVC.Controllers
{
    
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.IsAuthenticated = HttpContext.Session.GetString("AuthToken") != null;
            base.OnActionExecuting(context);
        }
    }
}
