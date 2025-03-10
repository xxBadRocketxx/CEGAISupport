using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CEGAISupport.Models;

namespace CEGAISupport.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey); // Sử dụng header chuẩn
        }

        public async Task<GeminiResponse> SendPromptAsync(GeminiRequest request)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent", content); // Endpoint chuẩn
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GeminiResponse>(jsonResponse);
        }
    }
}