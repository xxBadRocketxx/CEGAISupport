using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class IDCommandHandler : ICommandHandler
    {
        public async Task HandleAsync(string prompt, UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            // Lấy ID từ prompt
            string idString = ExtractId(prompt);

            if (string.IsNullOrEmpty(idString))
            {
                TaskDialog.Show("Error", "Could not find an ID in the prompt.");
                return;
            }

            if (int.TryParse(idString, out int elementId))
            {

                ElementId elemId = new ElementId(elementId);
                Element elem = doc.GetElement(elemId);

                if (elem != null)
                {
                    // Chọn phần tử
                    uiDoc.Selection.SetElementIds(new List<ElementId> { elemId });
                    uiDoc.ShowElements(elem); // Zoom đến phần tử

                    TaskDialog.Show("Selected Element by ID", $"Selected element with ID: {elementId}");
                }
                else
                {
                    TaskDialog.Show("Error", $"Could not find an element with ID: {elementId}");
                }

            }
            else
            {
                TaskDialog.Show("Error", "Invalid ID format.");
            }
        }

        private string ExtractId(string prompt)
        {

            // Tìm chuỗi số sau "ID"
            int index = prompt.IndexOf("ID", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                string remaining = prompt.Substring(index + 2).Trim(); // +2 để bỏ qua "ID"
                // Lấy chuỗi số
                string number = new string(remaining.TakeWhile(char.IsDigit).ToArray());
                return number;
            }

            return null;
        }
    }
}