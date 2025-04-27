using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PlastikMVC.AllDtos.ContentDtos;
using PlastikMVC.Helpers;
using PlastikMVC;

public class HttpClientForContent
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _baseUrl;

    public HttpClientForContent(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"];

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }

    public async Task<bool> CreateContentAsync(ContentAddDto contentDto)
    {
        using var formData = new MultipartFormDataContent();
       
        formData.Add(new StringContent(contentDto.Title ?? ""), "Title");
        formData.Add(new StringContent(contentDto.Summary ?? ""), "Summary");
        formData.Add(new StringContent(contentDto.Body ?? ""), "Body");
        formData.Add(new StringContent(contentDto.Slug ?? ""), "Slug");
        formData.Add(new StringContent(contentDto.IsFeatured.ToString()), "IsFeatured");
        formData.Add(new StringContent(contentDto.ScheduledPublishDate.ToString() ?? ""), "ScheduledPublishDate");
        formData.Add(new StringContent(contentDto.ContentSource ?? "Bilinmiyor"), "ContentSource");        
        formData.Add(new StringContent(contentDto.MediaType.ToString()), "MediaType");

      
        var mediasJson = JsonSerializer.Serialize(contentDto.Medias, _jsonOptions);

        formData.Add(new StringContent(mediasJson, Encoding.UTF8, "application/json"), "Medias");

        
        if (contentDto.MediaFiles != null)
        {
            foreach (var file in contentDto.MediaFiles)
            {
                var fileStream = new StreamContent(file.OpenReadStream());
                fileStream.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                formData.Add(fileStream, "MediaFiles", file.FileName);
            }
        }

        var response = await _httpClient.PostAsync($"{_baseUrl}/Content/CreateContent", formData);

        var responseContent = await response.Content.ReadAsStringAsync();
       

        return response.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateContentAsync(int id, ContentUpdateDto contentDto)
    {
        using var formData = new MultipartFormDataContent();

        //formData.Add(new StringContent(contentDto.Id.ToString()), "Id");
        formData.Add(new StringContent(contentDto.Title ?? ""), "Title");
        formData.Add(new StringContent(contentDto.Summary ?? ""), "Summary");
        formData.Add(new StringContent(contentDto.Body ?? ""), "Body");
        formData.Add(new StringContent(contentDto.Slug ?? ""), "Slug");
        formData.Add(new StringContent(contentDto.IsFeatured.ToString()), "IsFeatured");
        formData.Add(new StringContent(contentDto.ContentSource ?? "Bilinmiyor"), "ContentSource");
        formData.Add(new StringContent(contentDto.MediaType.ToString()), "MediaType");

        if (contentDto.ScheduledPublishDate.HasValue)
        {
            formData.Add(new StringContent(contentDto.ScheduledPublishDate.Value.ToString("o")), "ScheduledPublishDate");
        }

        if (contentDto.MediaFiles != null && contentDto.MediaFiles.Any())
        {
            foreach (var file in contentDto.MediaFiles)
            {
                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                formData.Add(streamContent, "MediaFiles", file.FileName);
            }
        }

        var response = await _httpClient.PutAsync($"{_baseUrl}/Content/UpdateContent/{id}", formData);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Hata: {errorMessage}");
        }

        return response.IsSuccessStatusCode;
    }
   
    public async Task<bool> DeleteContentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/Content/DeleteContent/{id}");
        return response.IsSuccessStatusCode;
    }
  
    public async Task<List<ContentGetAllDto>> GetAllContentsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetAllContents");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponse);
        return JsonSerializer.Deserialize<List<ContentGetAllDto>>(jsonResponse, _jsonOptions);

    }
  
    public async Task<List<ContentGetAllDto>> GetContentsByCategoryAsync(int categoryId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetContentsByCategory/{categoryId}");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContentGetAllDto>>(jsonResponse, _jsonOptions);
    }
   
    public async Task<ContentGetByIdDto> GetContentByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetContentById/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContentGetByIdDto>(jsonResponse, _jsonOptions);
    }
    
    public async Task<List<ContentGetAllDto>> GetFeaturedContentsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetFeaturedContents");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContentGetAllDto>>(jsonResponse, _jsonOptions);
    }
   
    public async Task<List<ContentGetAllDto>> GetLatestContentsAsync(int count)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetLatestContents/{count}");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContentGetAllDto>>(jsonResponse, _jsonOptions);
    }

    public async Task<PagedResult<ContentGetAllDto>> GetPaginatedContentsAsync(int page, int pageSize)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetPaginatedContents?page={page}&pageSize={pageSize}");

        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PagedResult<ContentGetAllDto>>(jsonResponse, _jsonOptions);
    }

    public async Task<List<ContentGetAllDto>> GetScheduledContentsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetScheduledContents");
        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContentGetAllDto>>(jsonResponse, _jsonOptions);
    }
    public async Task<ContentGetByIdDto> GetContentBySlugAsync(string slug)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Content/GetContentBySlug/{slug}");

        if (!response.IsSuccessStatusCode) return null;

        var jsonResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ContentGetByIdDto>(jsonResponse, _jsonOptions);
    }
}
