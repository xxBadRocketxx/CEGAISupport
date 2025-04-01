// Nếu bạn muốn parse đầy đủ response từ Gemini, bạn có thể dùng model này.
// Tuy nhiên, trong code hiện tại, bạn đang dùng `dynamic` nên model này không bắt buộc.
namespace CEGAISupport.Models
{
    public class GeminiResponse
    {
        // Bạn cần định nghĩa các property tương ứng với cấu trúc JSON response của Gemini.
        // Ví dụ:
        // public List<Candidate> Candidates { get; set; }
        // public PromptFeedback PromptFeedback { get; set; }
        public string Text { get; set; } // Thêm property này
    }

    // public class Candidate
    // {
    //     public Content Content { get; set; }
    //     public string FinishReason { get; set; }
    //     public int Index { get; set; }
    //     public List<SafetyRating> SafetyRatings { get; set; }
    // }

    // public class Content
    // {
    //     public List<Part> Parts { get; set; }
    //     public string Role { get; set; }
    // }

    // public class Part
    // {
    //     public string Text { get; set; }
    // }

    // public class SafetyRating
    // {
    //     public string Category { get; set; }
    //     public string Probability { get; set; }
    // }

    // public class PromptFeedback
    // {
    //     public List<SafetyRating> SafetyRatings { get; set; }
    // }
}