using PlastikMVC.AllDtos.CustomerRequestDtos;
using System.Text;
using System.Text.Json;

namespace PlastikMVC.HttpClientService.HttpClientForCustomerRequests
{
    public class HttpClientForCustomerRequests
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpClientForCustomerRequests> _logger;
        private readonly string _baseUrl;

        public HttpClientForCustomerRequests(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<HttpClientForCustomerRequests> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }
        public async Task<CustomerRequestDto> CreateCustomerRequestAsync(CustomerRequestAddDto requestDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/CustomerRequest/CreateCustomerRequest", requestDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerRequestDto>();
        }

        public async Task<CustomerRequestResponseDto> GetCustomerRequestByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<CustomerRequestResponseDto>($"{_baseUrl}/CustomerRequest/GetRequestById/{id}");
        }

        public async Task<CustomerRequestDto> GetCustomerRequestsByUserIdAsync(string userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<CustomerRequestDto>($"{_baseUrl}/CustomerRequest/GetRequestByUserId/{userId}");
        }

        public async Task<CustomerRequestUpdateDto> UpdateCustomerRequestAsync(int id, CustomerRequestUpdateDto requestDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/CustomerRequest/UpdateRequest/{id}", requestDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerRequestUpdateDto>();
        }

        public async Task<bool> DeleteCustomerRequestAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/CustomerRequest/DeleteRequest/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<CustomerRequestResponseDto> AssignCustomerRequestAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsync($"{_baseUrl}/CustomerRequest/AssignCustomer/{id}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerRequestResponseDto>();
        }

        public async Task<List<CustomerRequestDto>> GetUnassignedRequestsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<CustomerRequestDto>>($"{_baseUrl}/CustomerRequest/GetUnassigned");
        }

        public async Task<List<CustomerRequestDto>> GetCustomerRequestsByStatusAsync(CustomerRequestDto requestDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetFromJsonAsync<List<CustomerRequestDto>>($"{_baseUrl}/CustomerRequest/GetCustomerStatus");
            return response;
        }
        public async Task<List<CustomerRequestResponseDto>> GetAllCustomerRequests()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/CustomerRequest/GetAllCustomerRequests");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API request is failed: {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();

            var getAllRequests = JsonSerializer.Deserialize<List<CustomerRequestResponseDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return getAllRequests ?? new List<CustomerRequestResponseDto>();
        }
        public async Task<CustomerRequestDto> CreatePurposalRequestAsync(CustomerRequestAddDto requestDto)
        {
            try
            {
                // **Session'dan Token Al**
                string token = string.Empty;

                if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("AuthToken", out token) && string.IsNullOrEmpty(token))
                {
                    throw new Exception("AuthToken boş! Kullanıcı giriş yapmamış olabilir.");
                }

                // **Header'a Token Ekle**
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // **JSON Formatında Gönderilecek Veriyi Oluştur**
                var content = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.UTF8, "application/json");

                _logger.LogInformation("API'ye teklif oluşturma isteği gönderiliyor: {Url}", $"{_baseUrl}/CustomerRequest/CreatePurposalRequest");

                // **API'ye POST İsteği Gönder**
                var response = await _httpClient.PostAsync($"{_baseUrl}/CustomerRequest/CreatePurposalRequest", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API isteği başarısız: {StatusCode}, Mesaj: {Message}", response.StatusCode, errorMessage);
                    throw new Exception($"API isteği başarısız oldu. Durum Kodu: {response.StatusCode}, Mesaj: {errorMessage}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("API'den başarıyla yanıt alındı.");

                return JsonSerializer.Deserialize<CustomerRequestDto>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new Exception("Yanıt verisi boş.");

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP isteği sırasında hata oluştu.");
                throw new Exception("HTTP isteği sırasında hata oluştu.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Teklif oluşturma sırasında hata oluştu.");
                throw new Exception("Teklif oluşturma sırasında hata oluştu. Lütfen tekrar deneyiniz.", ex);
            }
        }
    }
}
