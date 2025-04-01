using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CEGAISupport.Models;

namespace CEGAISupport.Services
{
    public class GeminiService
    {
        private const string GeminiApiKey = "YOUR_ACTUAL_API_KEY"; // Thay YOUR_ACTUAL_API_KEY bằng API Key thật
        private const string GeminiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=";
        private readonly HttpClient _httpClient;

        public GeminiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SendMessage(string prompt)
        {
            // Tạo request payload ĐÚNG ĐỊNH DẠNG
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                // Thêm các tham số khác (nếu cần) vào ĐÂY, KHÔNG phải trong contents
                //generationConfig = new
                //{
                //    temperature = 0.7,
                //    maxOutputTokens = 800,
                //    topP = 0.9, // Ví dụ
                //    topK = 40   // Ví dụ
                //}
            };

            string jsonRequest = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, GeminiEndpoint + GeminiApiKey);
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API error: {response.StatusCode} - {errorContent}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Cách parse JSON response (dùng dynamic, đơn giản)
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);
            string generatedText = responseObject.candidates[0].content.parts[0].text;
            return generatedText;

            // Hoặc, nếu bạn muốn dùng GeminiResponse (ít linh hoạt hơn):
            // GeminiResponse geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(jsonResponse);
            // return geminiResponse.Text;
        }
    }
}