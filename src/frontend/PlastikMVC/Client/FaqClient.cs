using PlastikMVC.Client.Helpers;
using PlastikMVC.Client.Models.Request.FaqRequest;
using PlastikMVC.Client.Models.Response.FaqResponse;
using System.Text;
using System.Text.Json;

namespace PlastikMVC.Client
{
    public class FaqClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FaqClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

    
        public async Task<List<GetAllFaqResponse>> GetAllFaqsAsync()
        {
            return await SendRequestAsync<List<GetAllFaqResponse>>(HttpMethod.Get, "Faq");
        }

 
        public async Task<GetFaqByIdResponse> GetFaqByIdAsync(int id)
        {
            return await SendRequestAsync<GetFaqByIdResponse>(HttpMethod.Get, $"Faq/{id}");
        }

        public async Task CreateFaqAsync(CreateFaqRequest request)
        {
            await SendRequestAsync<object>(HttpMethod.Post, "Faq", request);
        }

        public async Task UpdateFaqAsync(int id, UpdateFaqRequest request)
        {
            await SendRequestAsync<object>(HttpMethod.Put, $"Faq/{id}", request);
        }

       
        public async Task DeleteFaqAsync(int id)
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"Faq/{id}");
        }
    }
}
