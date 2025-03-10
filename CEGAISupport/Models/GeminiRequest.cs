namespace CEGAISupport.Models
{
    public class GeminiRequest
    {
        public string Prompt { get; set; }
        public RevitModel RevitData { get; set; }

        // Có thể thêm các tham số khác cho Gemini API nếu cần
        // Ví dụ:
        // public string Model { get; set; } = "gemini-pro";
        // public float Temperature { get; set; } = 0.7f;
        // public int MaxOutputTokens { get; set; } = 200;
    }
}