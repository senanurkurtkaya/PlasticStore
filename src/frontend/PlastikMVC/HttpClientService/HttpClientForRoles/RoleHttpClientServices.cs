using Newtonsoft.Json;
using PlastikMVC.AllDtos.RoleDtos;
using PlastikMVC.Models;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PlastikMVC.HttpClientService.HttpClientForRoles
{
    public class RoleHttpClientServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;
        public RoleHttpClientServices(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
            _httpContextAccessor = httpContextAccessor;
        }

        // Yeni Rol Oluştur
        public async Task CreateRoleAsync(CreateRoleViewModel model, string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}/api/Role/CreateRole", jsonContent);


            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API isteği başarısız: {error}");
            }
        }

        // Tüm Rolleri Getir
        public async Task<List<GetAllRolesModel>> GetAllRolesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/Role/GetAllRoles");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API isteği başarısız oldu. Durum Kodu: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GetAllRolesModel>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<GetAllRolesModel>();
        }

        // ID ile Role Getir
        public async Task<GetRoleByIdModel> GetRoleByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id boş olamaz.", nameof(id));
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/api/Role/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API isteği başarısız oldu. Durum Kodu: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var role = JsonSerializer.Deserialize<GetRoleByIdModel>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (role == null)
            {
                throw new Exception("Rol bulunamadı!");
            }

            return role;
        }
        public async Task<List<RoleModel>> GetUserRolesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID boş olamaz.", nameof(userId));
            }

            var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];


            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_baseUrl}/role/get-roles/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API isteği başarısız oldu: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<RoleModel>>(content);

            return roles ?? new List<RoleModel>();
        }


        // Role Ata
        public async Task AssignRoleAsync(string userId, string roleId, string cookie)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"];

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var assignRoleDto = new
            {
                UserId = userId,
                RoleId = roleId
            };

            var content = new StringContent(JsonConvert.SerializeObject(assignRoleDto), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{_baseUrl}/Role/AssignRole", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Rol atanamadı: {errorMessage}");
            }
        }
        //Rolü geri al
        public async Task UnassignRoleAsync(string userId, string roleId, string token)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                UserId = userId,
                RoleId = roleId
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_baseUrl}/role/unassignrole", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API isteği başarısız: {error}");
            }
        }


    }
}
