using Autodesk.Revit.DB;
using Autodesk.Revit.UI; // Cần using để sử dụng UIDocument
using System;
using System.Linq;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class OpenSheetCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            string sheetName = ExtractSheetName(command); // Hàm trích xuất tên sheet

            if (string.IsNullOrEmpty(sheetName))
            {
                return "Please specify a sheet name or number to open (e.g., 'Open Sheet A101').";
            }

            // Tìm ViewSheet bằng tên hoặc số hiệu
            ViewSheet sheet = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .FirstOrDefault(s => s.SheetNumber.Equals(sheetName, StringComparison.OrdinalIgnoreCase) ||
                                     s.Name.Equals(sheetName, StringComparison.OrdinalIgnoreCase));

            if (sheet != null)
            {
                // Mở Sheet
                UIDocument uiDoc = new UIDocument(doc); // Tạo UIDocument
                uiDoc.ActiveView = sheet; // Đặt ActiveView thành sheet.
                return $"Sheet '{sheetName}' opened successfully.";
            }
            else
            {
                return $"Sheet '{sheetName}' not found.";
            }
        }
        private string ExtractSheetName(string command)
        {
            int sheetIndex = command.IndexOf("sheet", StringComparison.OrdinalIgnoreCase);
            int startIndex = (sheetIndex >= 0) ? sheetIndex + "sheet".Length : -1;
            return (startIndex >= 0) ? command.Substring(startIndex).Trim() : string.Empty;
        }
    }
}