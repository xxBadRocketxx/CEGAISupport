using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class ScopeBoxCommandHandler : ICommandHandler
    {
        public async Task HandleAsync(string prompt, UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            // Lấy tất cả các đối tượng nhìn thấy được trong view hiện hành
            FilteredElementCollector collector = new FilteredElementCollector(doc, doc.ActiveView.Id);
            ICollection<Element> visibleElements = collector.WhereElementIsNotElementType().ToElements();

            // Tính toán BoundingBox bao quanh các đối tượng
            BoundingBoxXYZ boundingBox = new BoundingBoxXYZ();
            foreach (Element elem in visibleElements)
            {
                BoundingBoxXYZ elemBox = elem.get_BoundingBox(doc.ActiveView);
                if (elemBox != null)
                {
                    if (boundingBox.Min == null || boundingBox.Max == null) //Trường hợp BoundingBox ban đầu rỗng
                    {
                        boundingBox.Min = elemBox.Min;
                        boundingBox.Max = elemBox.Max;
                    }
                    else
                    {
                        boundingBox.Min = new XYZ(
                        Math.Min(boundingBox.Min.X, elemBox.Min.X),
                        Math.Min(boundingBox.Min.Y, elemBox.Min.Y),
                        Math.Min(boundingBox.Min.Z, elemBox.Min.Z));

                        boundingBox.Max = new XYZ(
                        Math.Max(boundingBox.Max.X, elemBox.Max.X),
                        Math.Max(boundingBox.Max.Y, elemBox.Max.Y),
                        Math.Max(boundingBox.Max.Z, elemBox.Max.Z));
                    }
                }
            }

            // Tạo scope box
            using (Transaction t = new Transaction(doc, "Create Scope Box"))
            {
                t.Start();
                View3D view3D = doc.ActiveView as View3D;

                if (view3D != null && boundingBox.Min != null && boundingBox.Max != null)
                {
                    // Tạo scope box
                    ViewSection viewSection = ViewSection.CreateSection(doc, doc.ActiveView.GetTypeId(), boundingBox);
                    TaskDialog.Show("Tạo Scope Box", "Scope Box đã được tạo thành công.");
                }
                else
                {
                    TaskDialog.Show("Lỗi", "Không thể tạo Scope Box, không có đối tượng nào được chọn hoặc không phải view 3D.");
                }

                t.Commit();
            }
        }
    }
}