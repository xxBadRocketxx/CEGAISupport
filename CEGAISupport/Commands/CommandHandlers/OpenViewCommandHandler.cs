using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class OpenViewCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            string viewName = ExtractViewName(command);

            if (string.IsNullOrEmpty(viewName))
            {
                return "Please specify a view name to open (e.g., 'Open View Level 1').";
            }
            try
            {
                // Tìm View
                View view = new FilteredElementCollector(doc)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .FirstOrDefault(v => v.Name.Equals(viewName, StringComparison.OrdinalIgnoreCase));

                if (view != null)
                {
                    // Mở View
                    UIDocument uiDoc = new UIDocument(doc); // Tạo UIDocument
                    uiDoc.ActiveView = view; // Mở view
                    return $"View '{viewName}' opened successfully.";
                }
                else
                {
                    return $"View '{viewName}' not found.";
                }
            }
            catch (Exception ex)
            {
                return $"Error tạo schedule: {ex.Message}";
            }
        }
        private string ExtractViewName(string command)
        {
            int viewIndex = command.IndexOf("view", StringComparison.OrdinalIgnoreCase);
            int startIndex = (viewIndex >= 0) ? viewIndex + "view".Length : -1;
            return (startIndex >= 0) ? command.Substring(startIndex).Trim() : string.Empty;

        }
    }
}