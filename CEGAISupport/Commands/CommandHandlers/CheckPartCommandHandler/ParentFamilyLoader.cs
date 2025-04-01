// Commands/CommandHandlers/CheckPartCommandHandler/ParentFamilyLoader.cs
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler
{
    public class ParentFamilyLoader
    {
        public List<string> LoadParentFamilies(Document doc, string familyName)
        {
            // 1. Tìm tất cả các FamilyInstance của family có tên chứa searchText.
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                 .OfClass(typeof(FamilyInstance))
                 .WhereElementIsNotElementType();  // Chỉ lấy FamilyInstance, không lấy FamilySymbol

            // Lọc theo tên family (chú ý: lọc trên FamilySymbol của FamilyInstance)
            List<FamilyInstance> familyInstances = collector.Cast<FamilyInstance>()
                .Where(fi => fi.Symbol.FamilyName.IndexOf(familyName, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();


            // 2. Lấy danh sách các Family cha (Family) của các FamilyInstance vừa tìm được.
            HashSet<string> parentFamilies = new HashSet<string>();
            foreach (FamilyInstance fi in familyInstances)
            {

                if (fi.SuperComponent == null)
                {
                    // Loại bỏ tên family trùng với tên family đã nhập (KHÔNG phân biệt chữ hoa/thường)
                    if (!parentFamilies.Contains(fi.Symbol.FamilyName) && !fi.Symbol.FamilyName.Equals(familyName, StringComparison.OrdinalIgnoreCase)) //tránh trùng lặp và loại family đã nhập
                    {
                        parentFamilies.Add(fi.Symbol.FamilyName);
                    }
                }
                else
                {
                    FamilyInstance parent = doc.GetElement(fi.SuperComponent.Id) as FamilyInstance; // Lấy family cha trực tiếp
                    if (parent != null)
                    {
                        // Không cần kiểm tra trùng tên ở đây, vì đây là family cha
                        parentFamilies.Add(parent.Symbol.FamilyName); //thêm tên Family cha.
                    }
                }
            }
            return parentFamilies.ToList();
        }
    }
}