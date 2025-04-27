using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace PlastikMVC.Filters
{
    public class PlastikAuth : ActionFilterAttribute
    {
        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authToken = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "AuthToken").Value;
            var userRolesAsString = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "UserRoles").Value;

            if (string.IsNullOrEmpty(authToken))
            {
                context.Result = new RedirectResult("/unauthorized");
                base.OnActionExecuting(context);
                return;
            }
            else
            {
                var isTokenExpired = CheckTokenIsExpired(authToken);

                if (isTokenExpired)
                {
                    context.Result = new RedirectResult("/Account/Login");
                    base.OnActionExecuting(context);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(userRolesAsString) && !string.IsNullOrEmpty(Roles))
            {
                var roles = Roles.Split(",").ToList();
                var userRoles = JsonConvert.DeserializeObject<List<string>>(userRolesAsString);

                if (roles.Intersect(userRoles).Count() == 0)
                {
                    context.Result = new RedirectResult("/unauthorized");
                    base.OnActionExecuting(context);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authToken = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "AuthToken").Value;
            var userRolesAsString = context.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "UserRoles").Value;

            if (string.IsNullOrEmpty(authToken))
            {
                context.Result = new RedirectResult("/unauthorized");
                return base.OnActionExecutionAsync(context, next); 
            }

            if (!string.IsNullOrEmpty(userRolesAsString) && !string.IsNullOrEmpty(Roles))
            {
                var roles = Roles.Split(",").ToList();
                var userRoles = JsonConvert.DeserializeObject<List<string>>(userRolesAsString);

                if (roles.Intersect(userRoles).Count() == 0)
                {
                    context.Result = new RedirectResult("/unauthorized");
                    return base.OnActionExecutionAsync(context, next);
                }
            }

            return base.OnActionExecutionAsync(context, next);
        }
        private long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        private bool CheckTokenIsExpired(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var expired = tokenDate < now;

            return expired;
        }
    }
}
