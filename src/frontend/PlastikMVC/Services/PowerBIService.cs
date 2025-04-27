using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PlastikMVC.Services
{
    public class PowerBiService
    {
        private readonly string ClientId = "<YOUR_CLIENT_ID>";
        private readonly string ClientSecret = "<YOUR_CLIENT_SECRET>";
        private readonly string TenantId = "<YOUR_TENANT_ID>";
        private readonly string BaseApiUrl = "https://api.powerbi.com/v1.0/myorg/";

        public async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();
            var url = $"https://login.microsoftonline.com/{TenantId}/oauth2/v2.0/token";

            var body = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("scope", "https://analysis.windows.net/powerbi/api/.default")
            });

            var response = await client.PostAsync(url, body);
            var content = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(content);
            return json.GetProperty("access_token").GetString();
        }

        public async Task<string> GetEmbedUrlAsync(string reportId)
        {
            var accessToken = await GetAccessTokenAsync();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"{BaseApiUrl}reports/{reportId}");
            var content = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(content);

            return json.GetProperty("embedUrl").GetString();

        }

        public async Task CreateDatasetAsync(object datasetDefinition)
        {
            var accessToken = await GetAccessTokenAsync();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonContent = new StringContent(JsonSerializer.Serialize(datasetDefinition), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{BaseApiUrl}datasets", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Dataset oluşturulamadı: " + response.StatusCode);
            }
        }
    }
}
