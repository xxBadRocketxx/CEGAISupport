using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CEGAISupport.Utils
{
    public static class RevitUtils
    {
        // Ví dụ: Lấy Document hiện tại
        public static Document GetCurrentDocument(UIApplication uiApp)
        {
            return uiApp.ActiveUIDocument.Document;
        }
        // Các hàm tiện ích khác liên quan đến Revit API có thể được thêm vào đây.
    }
}