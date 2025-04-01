using System.Windows.Media;  // Cho Brush

namespace CEGAISupport.Models
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush ForegroundColor { get; set; }
        // Có thể thêm các thuộc tính khác (ví dụ: Timestamp, Sender, ...)
    }
}