
using PlastikMVC.Dtos.CategoryDto;
using PlastikMVC.Client.Models.Request.ProductRequest;
using PlastikMVC.Client.Models.Response;
using PlastikMVC.Client.Models.Response.ProductResponse;
using System.Text.Json;
using PlastikMVC.Exceptions;
using System.Text;
using PlastikMVC.Client.Helpers;

namespace PlastikMVC.Client
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<T> SendRequestAsync<T>(HttpMethod method, string url, object? content = null)
         {
            var request = new HttpRequestMessage(method, $"api/{url}");

            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, request);

            if (content != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(content), new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));

            }
            try
            {
                // HTTP isteğini gönder
                var response = await _httpClient.SendAsync(request);

                // Başarısız yanıt durumlarını ele al
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException($"Request to {url} failed with status {response.StatusCode}. Error: {errorContent}");
                    }

                    throw new Exception($"Request to {url} failed with status {response.StatusCode}. Error: {errorContent}");
                }

                // Yanıt beklemeyen durumlar için
                if (typeof(T) == typeof(object))
                {
                    return default!;// Yanıt beklenmiyor, geri dönüş yok
                }

                // Yanıt bekleyen durumlar için

                var result = await response.Content.ReadFromJsonAsync<T>();

                if (result == null)
                {
                    throw new Exception($"No data  returned from {url}");
                }
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Request to {url} failed.", ex);
            }
        }

       
        public async Task<List<GetAllProductResponse>> GetAllProductsAsync()
        {
            return await SendRequestAsync<List<GetAllProductResponse>>(HttpMethod.Get, "products");
        }
     
        public async Task<GetProductByIdResponse> GetProductByIdAsync(int id)
        {
            return await SendRequestAsync<GetProductByIdResponse>(HttpMethod.Get, $"products/{id}");
        }

        public async Task CreateProductAsync(CreateProductRequest request)
        {
           
            await SendRequestAsync<CreateProductResponse>(HttpMethod.Post, "products", request);
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, _httpClient);

            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request to api/products/{id} failed with status {response.StatusCode}. Error: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task DeleteProductAsync(int id )
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"products/{id}");
        }

        //____----------------------------------------------------------------


        // ✅ Belirli bir ürüne ait metatagları getir
        public async Task<ProductMetaTagsResponse> GetMetaTagsByProductIdAsync(int id)
        {
            return await SendRequestAsync<ProductMetaTagsResponse>(HttpMethod.Get, $"products/{id}/metaTags");
        }

       

        // ✅ Yeni bir metatag ekle
        public async Task CreateMetaTagsAsync(CreateProductMetaTagsRequest request)
        {
            await SendRequestAsync<object>(HttpMethod.Post, $"products/{request.ProductId}/metaTags", request);
        }

        // ✅ Metatag güncelle
        public async Task<bool> UpdateMetaTagAsync(int metaTagId, UpdateProductMetaTagsRequest request)
        {
            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, _httpClient);
            var response = await _httpClient.PutAsJsonAsync($"api/products/{request.ProductId}/metaTags", request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request to api/products/{request.ProductId}/metaTags failed with status {response.StatusCode}. Error: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }


        public async Task DeleteMetaTagsAsync(int metaTagId)
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"api/products/{metaTagId}/metatags");
        }


        //---------------------
        public async Task<List<ProductImageResponse>> GetImagesByProductIdAsync(int productId) //Belirli bir ürüne ait resimleri getir
        {
            return await SendRequestAsync<List<ProductImageResponse>>(HttpMethod.Get, $"Products/{productId}/ProductImages");
        }

        public async Task AddImageAsync(CreateProductImageRequest request) // Yeni bir ürün resmi ekle
        {
            await SendRequestAsync<object>(HttpMethod.Post, $"Products/{request.ProductId}/ProductImages", request);
        }

        public async Task<bool> UpdateImageAsync(int productId, int imageId, UpdateProductImageRequest request)
        //Belirli bir ürüne ait belirli bir resmi güncelle
        {
            HttpClientHelpers.AddAuthHeader(_httpContextAccessor, _httpClient);
            var response = await _httpClient.PutAsJsonAsync($"api/Products/{productId}/ProductImages/{imageId}", request);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task DeleteImageAsync(int productId, int imageId) // Belirli bir ürüne ait belirli bir resim sil
        {
            await SendRequestAsync<object>(HttpMethod.Delete, $"Products/{productId}/ProductImages/{imageId}");
        }

        public async Task<List<GetProductsByCategoryIdResponse>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await SendRequestAsync<List<GetProductsByCategoryIdResponse>>(HttpMethod.Get, $"categories/{categoryId}/products");
        }

        public async Task<List<GetTop20ProductResponse>> GetTop20Products()
        {
            return await SendRequestAsync<List<GetTop20ProductResponse>>(HttpMethod.Get, $"Products/Top20");
        }
    }
}

