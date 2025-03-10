namespace CEGAISupport.Models
{
    public class GeminiResponse
    {
        public string Text { get; set; } // Nội dung trả về từ Gemini
                                         // Thêm các thuộc tính khác tùy thuộc vào response của Gemini
                                         // Ví dụ:
        public string ErrorMessage { get; set; } // Thông báo lỗi (nếu có)
        // public List<string> Candidates { get; set; } // Nếu Gemini trả về nhiều lựa chọn
    }
}