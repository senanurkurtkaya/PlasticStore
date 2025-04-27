//using PlastikMVC.AllDtos.ContentDtos;
//using PlastikMVC.AllDtos.ContentMediaDtos;
//using System.Text;
//using System.Text.Json;

//namespace PlastikMVC.HttpClientService.HttpClientForContentMedias
//{
//    public class ContentMediaHttpClientService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _apiBaseUrl;
//        private readonly JsonSerializerOptions _jsonOptions;

//        public ContentMediaHttpClientService(HttpClient httpClient, IConfiguration configuration)
//        {
//            _httpClient = httpClient;
//            _jsonOptions = new JsonSerializerOptions
//            {
//                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//                WriteIndented = true
//            };
//            //_apiBaseUrl = configuration["ApiBaseUrl"] + "/api/ContentMedia"; // appsettings.json'dan API URL çekiliyor
//        }       
//        public async Task<bool> CreateContentMediaAsync(ContentMediaAddDto contentMediaAddDto)
//        {
//            using var formData = new MultipartFormDataContent();

//            // Ana içerik verilerini JSON olarak form-data'ya ekleyelim
//            var jsonContent = JsonSerializer.Serialize(contentMediaAddDto, _jsonOptions);

//            var jsonStringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

//            formData.Add(jsonStringContent, "contentDto");

//            // Eğer medya dosyaları varsa, bunları ekleyelim
//            if (contentDto.MediaFiles != null && contentDto.MediaFiles.Count > 0)
//            {
//                foreach (var file in contentDto.MediaFiles)
//                {
//                    var streamContent = new StreamContent(file.OpenReadStream());
//                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
//                    formData.Add(streamContent, "MediaFiles", file.FileName);
//                }
//            }

//            // API'ye isteği gönderelim
//            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/Content/CreateContent", formData);

//            return response.IsSuccessStatusCode;
//        }        
//        public async Task<bool> UpdateContentMediaAsync(int id, ContentMediaUpdateDto mediaDto)
//        {
//            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/ContentMedia/UpdateMedia", mediaDto);
//            return response.IsSuccessStatusCode;
//        }
        
//        public async Task<bool> DeleteContentMediaAsync(int id)
//        {
//            var response = await _httpClient.DeleteAsync($"{_baseUrl}/ContentMedia/DeleteMedia/{id}");
//            return response.IsSuccessStatusCode;
//        }
       
//        public async Task<ContentMediaDto> GetContentMediaByIdAsync(int id)
//        {
//            return await _httpClient.GetFromJsonAsync<ContentMediaDto>($"{_baseUrl}/ContentMedia/GetMediaById/{id}");
//        }
      
//        public async Task<List<ContentMediaDto>> GetFeaturedContentMediasAsync()
//        {
//            return await _httpClient.GetFromJsonAsync<List<ContentMediaDto>>($"{_baseUrl}/ContentMedia/GetFeaturedContentMedia");
//        }
       
//        public async Task<List<ContentMediaDto>> GetLatestContentMediasAsync(int count)
//        {
//            return await _httpClient.GetFromJsonAsync<List<ContentMediaDto>>($"{_baseUrl}/ContentMedia/GetLatestContentMedias/{count}");
//        }
      
//        public async Task<List<ContentMediaDto>> GetAllMediasByContentIdAsync(int contentId)
//        {
//            return await _httpClient.GetFromJsonAsync<List<ContentMediaDto>>($"{_baseUrl}/ContentMedia/GetAllMediasByContentId/{contentId}");
//        }
//    }
//}
