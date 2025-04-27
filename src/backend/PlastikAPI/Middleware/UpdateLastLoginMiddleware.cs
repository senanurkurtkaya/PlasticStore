using BLL.AbstractServices;
using BLL.Dtos.UserReportsDto;

namespace PlastikAPI.Middleware
{
    public class UpdateLastLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public UpdateLastLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            if (context.User.Identity.IsAuthenticated)
            {

                var _userId = context.User.FindFirst("UserId")?.Value;

                if (!string.IsNullOrEmpty(_userId))
                {

                    var updateLastLoginDto = new UpdateLastLoginDto
                    {
                        UserId = _userId,
                        LastLoginDate = DateTime.UtcNow
                    };


                    await userService.UpdateLastLoginDateAsync(updateLastLoginDto);
                }
            }


            await _next(context);


        }
    }
}
