using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CEGAISupport.Services; // Đảm bảo có dòng này
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class ScheduleCommandHandler : ICommandHandler
    {
        private readonly GeminiService _geminiService;

        public ScheduleCommandHandler(GeminiService geminiService) // Constructor nhận GeminiService
        {
            _geminiService = geminiService;
        }
        public async Task HandleAsync(string prompt, UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            string elementType = ExtractElementType(prompt);

            if (string.IsNullOrEmpty(elementType))
            {
                TaskDialog.Show("Error", "Could not determine element type to schedule.");
                return;
            }

            // Tạo ViewSchedule
            using (Transaction t = new Transaction(doc, "Create Schedule"))
            {
                t.Start();
                ViewSchedule schedule = null;

                if (elementType.Equals("wall", StringComparison.OrdinalIgnoreCase) || elementType.Equals("tường", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Walls));
                }
                else if (elementType.Equals("door", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cửa", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Doors));
                }
                else if (elementType.Equals("column", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cột", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Columns));
                }
                else if (elementType.Equals("beam", StringComparison.OrdinalIgnoreCase) || elementType.Equals("dầm", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_StructuralFraming));
                }
                else if (elementType.Equals("floor", StringComparison.OrdinalIgnoreCase) || elementType.Equals("sàn", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Floors));
                }
                else if (elementType.Equals("window", StringComparison.OrdinalIgnoreCase) || elementType.Equals("cửa sổ", StringComparison.OrdinalIgnoreCase))
                {
                    schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Windows));
                }

                if (schedule != null)
                {
                    schedule.Name = $"{elementType} Schedule";  // Đặt tên cho Schedule
                    TaskDialog.Show("Create Schedule", $"Schedule for {elementType} created successfully.");

                    // Mở view schedule (Revit 2022+)
                    uiDoc.ActiveView = schedule;

                }

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