using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Threading.Tasks;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class PickCommandHandler : ICommandHandler
    {
        public async Task HandleAsync(string prompt, UIDocument uiDoc)
        {
            // Lựa chọn đối tượng
            Selection sel = uiDoc.Selection;
            Reference pickedRef = null;
            try
            {
                pickedRef = sel.PickObject(ObjectType.Element, "Pick an element");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // Người dùng hủy chọn
                return;
            }


            if (pickedRef != null)
            {
                Element elem = uiDoc.Document.GetElement(pickedRef);

                // Hiển thị thông tin (ví dụ)
                using (Transaction t = new Transaction(uiDoc.Document, "Show Element Info"))
                {
                    t.Start();
                    TaskDialog.Show("Picked Element", $"Element ID: {elem.Id}, Type: {elem.GetType().Name}");
                    t.Commit();
                }

            }
        }
    }
}