using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CEGAISupport.Services;
using CEGAISupport.Utils;  // Đảm bảo using này
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class CountCommandHandler : ICommandHandler // Implement interface
    {
        private readonly GeminiService _geminiService; // Dùng để gọi Gemini (nếu cần)

        public CountCommandHandler(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task HandleAsync(string prompt, UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            string elementType = ExtractElementType(prompt);

            if (string.IsNullOrEmpty(elementType))
            {
                TaskDialog.Show("Error", "Could not determine element type to count.");
                return;
            }

            int count = 0;
            if (elementType.Equals("wall", StringComparison.OrdinalIgnoreCase) || elementType.Equals("tường", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetWallDataList(doc).Count;
            }
            else if (elementType.Equals("door", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cửa", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetDoorDataList(doc).Count;
            }
            else if (elementType.Equals("column", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cột", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetColumnDataList(doc).Count;
            }
            else if (elementType.Equals("beam", StringComparison.OrdinalIgnoreCase) || elementType.Equals("dầm", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetBeamDataList(doc).Count;
            }
            else if (elementType.Equals("floor", StringComparison.OrdinalIgnoreCase) || elementType.Equals("sàn", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetFloorDataList(doc).Count;
            }
            else if (elementType.Equals("window", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cửa sổ", StringComparison.OrdinalIgnoreCase))
            {
                count = RevitUtils.GetWindowDataList(doc).Count;
            }
            // Thêm các case khác

            using (Transaction t = new Transaction(doc, "Count Elements"))
            {
                t.Start();
                TaskDialog.Show("Count Result", $"Number of {elementType}s: {count}");
                t.Commit();
            }
        }

        private string ExtractElementType(string prompt)
        {
            // Rút gọn logic trích xuất (có thể cải tiến bằng NLP sau)
            if (prompt.IndexOf("wall", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("tường", StringComparison.OrdinalIgnoreCase) >= 0) return "wall";
            if (prompt.IndexOf("door", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("cửa", StringComparison.OrdinalIgnoreCase) >= 0) return "door";
            if (prompt.IndexOf("column", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("cột", StringComparison.OrdinalIgnoreCase) >= 0) return "column";
            if (prompt.IndexOf("beam", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("dầm", StringComparison.OrdinalIgnoreCase) >= 0) return "beam";
            if (prompt.IndexOf("floor", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("sàn", StringComparison.OrdinalIgnoreCase) >= 0) return "floor";
            if (prompt.IndexOf("window", StringComparison.OrdinalIgnoreCase) >= 0 || prompt.IndexOf("cửa sổ", StringComparison.OrdinalIgnoreCase) >= 0) return "window";
            return null;
        }
    }
}