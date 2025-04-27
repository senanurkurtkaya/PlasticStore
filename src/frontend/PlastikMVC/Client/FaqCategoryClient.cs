using PlastikMVC.Client.Helpers;
using PlastikMVC.Client.Models.Request.FaqCategoryRequest;
using PlastikMVC.Client.Models.Response.FaqCategoryResponse;
using System.Text;
using System.Text.Json;

namespace PlastikMVC.Client
{
    public class FaqCategoryClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FaqCategoryClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<T?> SendRequestAsync<T>(HttpMethod method, string url, object? content = null)
        {
            var request = new HttpRequestMessage(method, $"api/{url}");

            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, request);

            if (content != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(content), new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            }

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Request to {url} failed with status {response.StatusCode}. Error: {errorContent}");
                }

                if (typeof(T) == typeof(object) || typeof(T) == typeof(void))
                {
                    return default;
                }

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

        public async Task<List<GetAllFaqCategoryResponse>> GetAllFaqCategoriesAsync()
        {
            return await SendRequestAsync<List<GetAllFaqCategoryResponse>>(HttpMethod.Get, "FaqCategory");
        }

        public async Task<GetFaqCategoryByIdResponse> GetFaqCategoryByIdAsync(int id)
        {
            return await SendRequestAsync<GetFaqCategoryByIdResponse>(HttpMethod.Get, $"FaqCategory/{id}/faqs");
        }

        public async Task CreateFaqCategoryAsync(CreateFaqCategoryRequest request)
        {
            await SendRequestAsync<object>(HttpMethod.Post, "FaqCategory", request);
        }

        public async Task UpdateFaqCategoryAsync(int id, UpdateFaqCategoryRequest request)
        {
            await SendRequestAsync<object>(HttpMethod.Put, $"FaqCategory/{id}", request);
        }

        public async Task DeleteFaqCategoryAsync(int id)
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"FaqCategory/{id}");
        }
    }
}
