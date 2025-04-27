using Microsoft.AspNetCore.Mvc;
using PlastikMVC.AllDtos.UserDtos;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PlastikMVC.HttpClientService.HttpClientForRoles
{
    public class UserHttpClientServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserHttpClientServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;
        public UserHttpClientServices(
            IHttpClientFactory httpClientFactory,
            ILogger<UserHttpClientServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        // Tüm kullanıcıları getir
        public async Task<IEnumerable<GetAllUsersModel>> GetAllUsers()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("AuthToken boş. Kullanıcı giriş yapmamış olabilir.");
                }

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("API isteği gönderiliyor: {Url}", $"{_baseUrl}/user/all-users");

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("⚠️  Kullanıcı giriş yapmamış, API çağrısı yapılmayacak.");
                    return new List<GetAllUsersModel>(); // Token yoksa boş liste dön
                }

                // ✅ **Token'ı HTTP Header'a Ekle**
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _logger.LogInformation("API isteği gönderiliyor: {Url}", $"{_baseUrl}/api/User/all-users");

                var response = await client.GetAsync($"{_baseUrl}/api/User/all-users");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API isteği başarısız: {StatusCode}, Mesaj: {Message}", response.StatusCode, errorMessage);
                    throw new Exception($"API isteği başarısız oldu. Durum Kodu: {response.StatusCode}, Mesaj: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API yanıtı başarıyla alındı.");

                var users = JsonSerializer.Deserialize<List<GetAllUsersModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? Enumerable.Empty<GetAllUsersModel>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP isteği sırasında hata oluştu.");
                throw new Exception("HTTP isteği sırasında hata oluştu.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcıları çekerken bir hata oluştu.");
                throw new Exception("Kullanıcıları çekerken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.", ex);
            }

            return new List<GetAllUsersModel>();
        }


        // ID'ye göre kullanıcı getir
        public async Task<GetUserByIdModel> GetUserById(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_baseUrl}/User/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API isteği başarısız oldu: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<GetUserByIdModel>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            return user;
        }
        public async Task<bool> UpdateUser(EditUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Kullanıcı verisi boş olamaz.");

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{_baseUrl}/User", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_baseUrl}/User/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RegisterUser(UserRegisterModel model)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_baseUrl}/User/Register", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"RegisterUser API çağrısı başarısız: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterUser sırasında bir hata oluştu.");
                throw;
            }
        }


        [HttpGet]
        public async Task<List<ActiveUsersDto>> GetDailyActiveUsersAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/User/GetDailyActiveUsers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Empty request has taken from API!");
            }

            var content = await response.Content.ReadAsStringAsync();

            var getDaily = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return getDaily ?? new List<ActiveUsersDto>();
        }
        [HttpGet]
        public async Task<List<ActiveUsersDto>> GetWeeklyActiveUsersAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/User/GetWeeklyActiveUsers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Empty request has taken from API!");
            }

            var content = await response.Content.ReadAsStringAsync();

            var weeklyReport = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return weeklyReport ?? new List<ActiveUsersDto>();
        }

        public async Task<List<ActiveUsersDto>> GetMonthlyActiveUsersAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/User/GetMonthlyActiveUsers");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Empty request has taken from API!");
            }

            var content = await response.Content.ReadAsStringAsync();

            var getMonthly = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return getMonthly ?? new List<ActiveUsersDto>();
        }

        public async Task<List<ActiveUsersDto>> GetUserStatisticsWithDetailsAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{_baseUrl}/User/GetUserStatisticsWithDetail");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Empty request has taken from API!");
            }

            var content = await response.Content.ReadAsStringAsync();

            var getUserStatisticsWithDetails = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return getUserStatisticsWithDetails ?? new List<ActiveUsersDto>();
        }


    }
}

