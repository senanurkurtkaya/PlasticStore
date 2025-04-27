using BLL.Dtos.ReportsDto;
using System.Net.Http;
using System.Text.Json;

namespace PlastikMVC.HttpClientService.HttpClientForReports
{
    public class HttpClientForReports
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;

        public HttpClientForReports(IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }

        public async Task<List<ProductSalesDto>> GetProductSalesAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = client.GetAsync($"{_baseUrl}/Dtos/ReportsDto/ProductSales");
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Sending request failed!");
            }
            var content =await response.Result.Content.ReadAsStringAsync();
            
            var productSales = JsonSerializer.Deserialize<List<ProductSalesDto>>(content,new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return productSales ?? new List<ProductSalesDto>();
        }
        public async Task<List<CategorySalesDto>> CategorySalesAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = client.GetAsync($"{_baseUrl}/Dtos/ReportsDto/CategorySales");

            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Sending request failed!");
            }
            var content =await response.Result.Content.ReadAsStringAsync();
            var categorySales = JsonSerializer.Deserialize<List<CategorySalesDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            return categorySales ?? new List<CategorySalesDto>();

        }
        public async Task<RevenueStatsDto> RevenueStatsAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = client.GetAsync($"{_baseUrl}/Dtos/ReportsDto/GetRevenueStats");

            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Sending request failed!");
            }

            var content =await response.Result.Content.ReadAsStringAsync();
            var revenueStats = JsonSerializer.Deserialize<RevenueStatsDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return revenueStats;
        }
        public async Task<List<ProductSalesDto>> TopFiveSales()
        {
            
                var client = _httpClientFactory.CreateClient();
               
                var response = await client.GetAsync($"{_baseUrl}/Dtos/ReportsDto/TopFiveSales");
            
                if (response.IsSuccessStatusCode)
                {
                    
                    var topFiveSales = await response.Content.ReadFromJsonAsync<List<ProductSalesDto>>();
                    return topFiveSales ?? new List<ProductSalesDto>();
                }
                
                throw new HttpRequestException($"API call failed with status code: {response.StatusCode}");      
        }

    }
}
