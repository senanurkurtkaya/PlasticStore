using Newtonsoft.Json;

namespace PlastikAPI.Middleware
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Session == null || context.Session.GetString("UserRoles") == null)
            {
                // Eğer Session yoksa veya UserRoles bulunmuyorsa giriş sayfasına yönlendir
                context.Response.Redirect("/Account/Login");
                return;
            }

            var roles = JsonConvert.DeserializeObject<List<string>>(context.Session.GetString("UserRoles"));
            if (!roles.Contains("Admin") && context.Request.Path.StartsWithSegments("/Admin"))
            {
                context.Response.Redirect("/Account/AccessDenied");
                return;
            }

            await _next(context);
        }



    }

}
