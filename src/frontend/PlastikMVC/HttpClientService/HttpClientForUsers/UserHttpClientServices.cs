//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using PlastikMVC.AllDtos.RoleDtos;
//using PlastikMVC.AllDtos.UserDtos;
//using PlastikMVC.Models;
//using System.Text;
//using System.Text.Json;

//namespace PlastikMVC.HttpClientService.HttpClientForUsers
//{
//    public class UserHttpClientServices
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        // UserController CREATED//ONR
//        public UserHttpClientServices(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }
//        public async Task<List<GetAllUsersModel>> GetAllUsers()
//        {

//            var client = _httpClientFactory.CreateClient();


//            var response = await client.GetAsync($"{_baseUrl}/User/GetAllUsers");  

//            if (!response.IsSuccessStatusCode)
//            {

//                throw new Exception($"API request is failed: {response.StatusCode}");
//            }

//            var content = await response.Content.ReadAsStringAsync();

//            var users = JsonSerializer.Deserialize<List<GetAllUsersModel>>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return users ?? new List<GetAllUsersModel>();
//        }
//        public async Task<GetUserByIdModel> GetUserById(string id)
//        {

//            if (string.IsNullOrWhiteSpace(id))
//            {
//                throw new ArgumentException("ID must not be empty", nameof(id));
//            }

//            var client = _httpClientFactory.CreateClient();

//            var response = await client.GetAsync($"{_baseUrl}/User/GetUserById");

//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception($"API request failed. {response.StatusCode}");
//            }
//            var content = await response.Content.ReadAsStringAsync();

//            if (string.IsNullOrWhiteSpace(content))
//            {
//                throw new Exception("Empty request has taken from API!");
//            }

//            var user = JsonSerializer.Deserialize<GetUserByIdModel>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            if (user == null)
//            {
//                throw new Exception("Empty request has taken from API!");
//            }

//            return user;
//        }
//        [HttpGet]
//        public async Task<ActionResult<List<ActiveUsersDto>>> GetDailyActiveUsersAsync()
//        {
//            var client = _httpClientFactory.CreateClient();

//            var response = await client.GetAsync($"{_baseUrl}/User/GetDailyActiveUsers");

//            if(!response.IsSuccessStatusCode)
//            {
//                throw new Exception("Empty request has taken from API!");
//            }
//            var content =await response.Content.ReadAsStringAsync();

//            var getDaily = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content,new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return getDaily ?? new List<ActiveUsersDto>(); 

//        }
//        [HttpGet]
//        public async Task<ActionResult<List<ActiveUsersDto>>> GetWeeklyActiveUsersAsync()
//        {
//            var client = _httpClientFactory.CreateClient();

//            var response =await client.GetAsync($"{_baseUrl}/User/GetWeeklyActiveUsers");

//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception("Empty request has taken from API!");
//            }

//            var content = await response.Content.ReadAsStringAsync();
//            var weeklyReport = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return weeklyReport ?? new List<ActiveUsersDto>();

//        }
//        [HttpGet]
//        public async Task<ActionResult<List<ActiveUsersDto>>> GetMonthlyActiveUsersAsync()
//        {
//            var client = _httpClientFactory.CreateClient();

//            var response = await client.GetAsync($"{_baseUrl}/User/GetMonthlyActiveUsers");

//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception("Empty request has taken from API!");
//            }

//            var content = await response.Content.ReadAsStringAsync();

//            var getMonthly = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return getMonthly ?? new List<ActiveUsersDto>();
//        }

//        [HttpGet]
//        public async Task<ActionResult<List<ActiveUsersDto>>> GetUserStatisticsWithDetailsAsync()
//        {
//            var client = _httpClientFactory.CreateClient();

//            var response =await client.GetAsync($"{_baseUrl}/User/GetUserStatisticsWithDetail");

//            if (!response.IsSuccessStatusCode)
//            {
//                throw new Exception("Empty request has taken from API!");
//            }

//            var content = await response.Content.ReadAsStringAsync();

//            var getUserStatistiscWithDetails = JsonSerializer.Deserialize<List<ActiveUsersDto>>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });

//            return getUserStatistiscWithDetails ?? new List<ActiveUsersDto>();
//        }

//    }
//}
