using System.Net.Http;

namespace PlastikMVC.Client.Helpers
{
    public static class HttpClientHelpers
    {
        public static void AddAuthHeader(IHttpContextAccessor httpClientAccessor, HttpRequestMessage httpRequestMessage)
        {
            var authToken = httpClientAccessor.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "AuthToken").Value;

            if (!string.IsNullOrEmpty(authToken))
            {
                httpRequestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
        }

        public static void AddAuthHeader(IHttpContextAccessor httpClientAccessor, HttpClient httpClient)
        {
            var authToken = httpClientAccessor.HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "AuthToken").Value;

            if (!string.IsNullOrEmpty(authToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
        }
    }
}
