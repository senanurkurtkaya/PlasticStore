using BLL.MappingProfiles;
using PlastikAPI.Extensions;
using PlastikAPI.Middleware;
using BLL;
using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusiness(builder.Configuration.GetConnectionString("DefaultConnection"));

// Services Configuration

builder.Services.AddCustomIdentity();
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomCors();
builder.Services.AddCustomSession();
builder.Services.AddLogging();
builder.Services.AddCustomAuthorization();


var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

app.Use((context, next) =>
{
    var authHeader = context.Request.Headers.Authorization;

    return next();
});

// Middleware Pipeline
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowSpecificOrigins");
app.UseDeveloperExceptionPage();



app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UpdateLastLoginMiddleware>();
//app.UseMiddleware<RoleAuthorizationMiddleware>();



// Middleware Pipeline
// app.UseCustomMiddlewares();
app.MapControllers();
app.Run();
