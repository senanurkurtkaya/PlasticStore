using PlastikAPI.Middleware;

namespace PlastikAPI.Extensions
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            // Swagger
            if (app.ApplicationServices.GetService<IHostEnvironment>()?.IsDevelopment() == true)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            // HTTP Redirection
            // app.UseHttpsRedirection();

            // Static Files
            app.UseStaticFiles();

            // Routing
            app.UseRouting();

            // Cookie Policy
            // app.UseCookiePolicy();

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Custom Middlewares
            app.UseMiddleware<UpdateLastLoginMiddleware>();
            app.UseMiddleware<RoleAuthorizationMiddleware>();

            return app;
        }
    }
}
            

        