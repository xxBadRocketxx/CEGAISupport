using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using CEGAISupport.Models;
using Autodesk.Revit.UI;
using CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class TextNoteSearchCommandHandler : ICommandHandler
    {
        private Dictionary<int, string> _viewToSheetMap; // Lưu trữ ánh xạ ViewId -> SheetName

        public string Execute(string command, Document doc)
        {
            return "This command handler is used internally by other command handlers."; // Lệnh này chỉ dùng nội bộ
        }

        // Phương thức chính để tìm kiếm TextNote, trả về Dictionary<TextNoteInfo, bool>
        public Dictionary<TextNoteInfo, bool> FindTextNotes(Document doc, string searchText, string sheetName = "")
        {
            _viewToSheetMap = CreateViewToSheetMap(doc); // Tạo ánh xạ View -> Sheet
            Dictionary<TextNoteInfo, bool> textNotesInfo = new Dictionary<TextNoteInfo, bool>(); // Kết quả trả về

            // Lấy danh sách ParameterInfo để so sánh
            FamilyParameterLoader parameterLoader = new FamilyParameterLoader();
            List<ParameterInfo> parameterInfos = parameterLoader.LoadFamilyParameters(doc, searchText);

            // 1. Lọc Sheet theo parameter "DISCIPLINE"
            List<ViewSheet> targetSheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(s =>
                {
                    Parameter disciplineParam = s.LookupParameter("DISCIPLINE"); // Tìm parameter DISCIPLINE
                    if (disciplineParam != null && disciplineParam.StorageType == StorageType.String)
                    {
                        string disciplineValue = disciplineParam.AsString(); // Lấy giá trị của parameter
                        return disciplineValue != null &&
                               (disciplineValue.IndexOf("DETAILS", StringComparison.OrdinalIgnoreCase) >= 0); // Lọc theo giá trị "DETAILS"
                    }
                    return false;
                })
                .ToList();

            // 2. Duyệt qua các Sheet đã lọc
            foreach (ViewSheet sheet in targetSheets)
            {
                // 3. Lấy tất cả View trên Sheet
                ICollection<ElementId> viewIds = sheet.GetAllPlacedViews();

                // 4. Duyệt qua tất cả các View trên Sheet
                foreach (ElementId viewId in viewIds)
                {
                    View view = doc.GetElement(viewId) as View; // Lấy View từ ElementId
                    if (view == null || view.IsTemplate) continue; // Bỏ qua View template

                    // 5. Tìm TextNote trong View
                    FilteredElementCollector textCollector = new FilteredElementCollector(doc, view.Id)
                        .OfClass(typeof(TextNote));

                    foreach (TextNote textNote in textCollector)
                    {
                        // Kiểm tra xem TextNote có chứa searchText không (không phân biệt hoa/thường)
                        if (textNote.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string viewSheetName = GetSheetNameForView(view.Id); // Lấy tên Sheet chứa View
                            // Tạo đối tượng TextNoteInfo
                            TextNoteInfo info = new TextNoteInfo
                            {
                                ViewName = view.Name,
                                SheetName = viewSheetName,
                                Text = textNote.Text
                            };

                            // Xử lý văn bản TextNote: bỏ dòng đầu, thay xuống dòng bằng dấu cách
                            string[] lines = textNote.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            string processedTextNoteText = string.Join(" ", lines.Skip(1));

                            bool matchFound = false; // Biến kiểm tra xem có khớp với parameter nào không
                            // So sánh với từng ParameterInfo
                            foreach (ParameterInfo paramInfo in parameterInfos)
                            {
                                // So sánh không phân biệt hoa/thường
                                if (processedTextNoteText.IndexOf(paramInfo.Value, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    matchFound = true; // Đánh dấu là khớp
                                    break; // Thoát vòng lặp nếu tìm thấy khớp
                                }
                            }

                            textNotesInfo[info] = matchFound; // Thêm vào dictionary kết quả so sánh
                        }
                    }
                }
            }

            return textNotesInfo; // Trả về dictionary
        }
        // Hàm tạo ánh xạ ViewId -> "SheetNumber - SheetName"
        private Dictionary<int, string> CreateViewToSheetMap(Document doc)
        {
            Dictionary<int, string> viewToSheetMap = new Dictionary<int, string>();

            // Lấy tất cả Sheet
            FilteredElementCollector sheetCollector = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet));

            foreach (ViewSheet sheet in sheetCollector)
            {
                // Lấy tất cả View trên Sheet
                ICollection<ElementId> viewIds = sheet.GetAllPlacedViews();

                foreach (ElementId viewId in viewIds)
                {
                    // Thêm vào dictionary (key là ViewId, value là "SheetNumber - SheetName")
                    if (!viewToSheetMap.ContainsKey(viewId.IntegerValue))
                    {
                        viewToSheetMap.Add(viewId.IntegerValue, sheet.SheetNumber + " - " + sheet.Name);
                    }
                }
            }
            return viewToSheetMap;
        }

        // Hàm lấy tên Sheet từ ViewId (dựa vào ánh xạ đã tạo)
        private string GetSheetNameForView(ElementId viewId)
        {
            if (_viewToSheetMap != null && _viewToSheetMap.ContainsKey(viewId.IntegerValue))
            {
                return _viewToSheetMap[viewId.IntegerValue]; // Trả về tên Sheet
            }
            return "N/A"; // Trả về "N/A" nếu không tìm thấy
        }
    }
}