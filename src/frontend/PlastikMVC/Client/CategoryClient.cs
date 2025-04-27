using PlastikMVC.Client.Helpers;
using PlastikMVC.Client.Models.Request.CategoryRequest;
using PlastikMVC.Client.Models.Response.CategoryResponse;
using System;
using System.Text.Json;

namespace PlastikMVC.Client
{
    public class CategoryClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<T?>SendRequestAsync<T>(HttpMethod method, string url, object? content = null)
        {
            // HTTP isteği oluştur
            var request = new HttpRequestMessage(method, $"api/{url}");

            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, request);

            // Gövde (content) varsa, JSON içerik olarak ekle
            if (content != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(content), new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
                
            }

            try
            {
                // HTTP isteğini gönder
                var response = await _httpClient.SendAsync(request);

                // Yanıtın başarılı olup olmadığını kontrol et
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Request to {url} failed with status {response.StatusCode}. Error: {errorContent}");
                }

                // Yanıt beklenmiyorsa (örneğin veri döndürmeyen `DELETE`)
                if (typeof(T) == typeof(object) || typeof(T) == typeof(void))
                {
                    return default; // Yanıt beklenmiyor, default döndür
                }

                // Yanıt bekleniyorsa, JSON'u `T` türüne dönüştür
                var result = await response.Content.ReadFromJsonAsync<T>();

                if (result == null)
                {
                    throw new Exception($"No data returned from {url}");
                }
                 
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Request to {url} failed.", ex);
            }
        }

        public async Task<List<GetAllCategoryResponse>> GetAllCategoriesAsync() //tüm kategorileri getirir response dönmeli
        {
            return await SendRequestAsync<List<GetAllCategoryResponse>>(HttpMethod.Get, "categories");
        }

        public async Task<GetCategoryByIdResponse> GetCategoryByIdAsync(int id)  //belirli bir ID ye göre getirir  response dönmeli
        {
            return await SendRequestAsync<GetCategoryByIdResponse>(HttpMethod.Get, $"categories/{id}");
        }

        public async Task CreateCategoryAsync(CreateCategoryRequest request) // yrni katregori oluşturr  sadece başarılı durumu dogrular
        {
            await SendRequestAsync<CreateCategoryResponse>(HttpMethod.Post, "categories", request);
        }

        public async Task UpdateCategoryAsync(int id, UpdateCategoryRequest request) // genellikle  güncelleme işlemleri veri döndürmez 
        {
            await SendRequestAsync<object>(HttpMethod.Put, $"categories/{id}", request);
        }

        public async Task DeleteCategoryAsync(int id) // genellikle başarılı durumu döndürür veri döndürmeye gerrek yok
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"categories/{id}");
        }
    }
}

