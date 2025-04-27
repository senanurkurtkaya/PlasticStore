
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlastikMVC.Client;
using PlastikMVC.HttpClientService.HttpClientForRoles; // HttpClient servisleri için gerekli
using System.Text;

using PlastikMVC.Client;
using PlastikMVC;
using PlastikMVC.HttpClientService.HttpClientForRoles;
using System.Text.Json.Serialization;
using PlastikMVC.HttpClientService.HttpClientForCustomerRequests;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<HttpClientForContent>();
builder.Services.AddHttpClient<UserHttpClientServices>();
builder.Services.AddHttpClient<HttpClientForCustomerRequests>();

builder.Services.AddHttpContextAccessor();
// Veritabanı bağlantı dizesini alın
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// AddHttpClient ile HttpClient servislerini kaydedin
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

//builder.Services.AddHttpClient<RoleHttpClientServices>(httpClient => new RoleHttpClientServices(httpClient, baseUrl));

builder.Services.AddHttpClient<UserHttpClientServices>(client =>
{
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new Exception("ApiSettings:BaseUrl appsettings.json'da tanımlı değil.");
    }
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<RoleHttpClientServices>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    
    if (string.IsNullOrEmpty(baseUrl))
    {
        throw new Exception("ApiSettings:BaseUrl appsettings.json'da tanımlı değil.");
    }
    client.BaseAddress = new Uri($"{baseUrl}/Role"); // Role API endpointi
});

//builder.Services.AddHttpClient<HttpClientForContent>(client =>
//{
//    client.BaseAddress = new Uri(_baseUrl);
//});
// Kullanıcı servisini kaydedin
builder.Services.AddScoped<UserHttpClientServices>();
builder.Services.AddScoped<RoleHttpClientServices>();
builder.Services.AddScoped<HttpClientForCustomerRequests>();

//builder.Services.AddControllersWithViews()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//    });
// Session'ı etkinleştir
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 dakika boyunca aktif
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // GDPR uyumluluğu için gerekli
});

// HttpContext.Session için DistributedMemoryCache ekleyin
builder.Services.AddDistributedMemoryCache();

// JWT Authentication yapılandırması
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JwtSettings:SecretKey appsettings.json dosyasında eksik. Lütfen kontrol edin.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        //RoleClaimType = "role",
        //NameClaimType = "UserId"
    };
});

//// Yetkilendirme politikaları
//builder.Services.AddAuthorization(options =>
//{
//    // Admin veya Müşteri Temsilcisi için izin veren politika
//    options.AddPolicy("CustomerServicePolicy", policy =>
//        policy.RequireAssertion(context =>
//            context.User.IsInRole("Müşteri Temsilcisi") || context.User.IsInRole("Admin")));

//    // Admin veya İçerik Yöneticisi için izin veren politika
//    options.AddPolicy("ContentManagerPolicy", policy =>
//        policy.RequireAssertion(context =>
//            context.User.IsInRole("İçerik Yöneticisi") || context.User.IsInRole("Admin")));

//    // Sadece Admin için izin veren politika
//    options.AddPolicy("AdminPolicy", policy =>
//        policy.RequireRole("Admin"));
//});

builder.Services.AddHttpClient<ProductClient>(httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<CategoryClient>(httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<FaqCategoryClient>(httpClient => httpClient.BaseAddress = new Uri(baseUrl));
builder.Services.AddHttpClient<FaqClient>(httpClient => httpClient.BaseAddress = new Uri(baseUrl));

// MVC Controller'ları ve diğer servisler
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ProductClient>();
builder.Services.AddHttpClient<CategoryClient>();
builder.Services.AddScoped<HttpClientForCustomerRequests>();

builder.Services.AddHttpClient<CategoryClient>();
var app = builder.Build();

//app.UseStatusCodePagesWithRedirects("/Home/AccessDenied");

// Uygulama pipeline yapılandırması
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Geliştirme ortamı için hata ayıklama sayfası
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security
}


app.UseHttpsRedirection(); // HTTPS yönlendirme
app.UseStaticFiles(); // Statik dosyalar

app.UseRouting(); // Yönlendirme
app.UseSession(); // Session kullanımı
app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization(); // Yetkilendirme

// Varsayılan route tanımları
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
