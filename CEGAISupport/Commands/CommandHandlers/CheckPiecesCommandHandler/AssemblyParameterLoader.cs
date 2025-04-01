using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using CEGAISupport.Models;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class AssemblyParameterLoader
    {
        public List<ParameterInfo> LoadSheetParameters(ViewSheet sheet)
        {
            if (sheet == null)
            {
                return new List<ParameterInfo>();
            }

            List<ParameterInfo> parameterInfos = new List<ParameterInfo>();

            // Lấy giá trị BuiltInParameter TỪ SHEET (dựa vào code bạn cung cấp).
            parameterInfos.Add(GetBuiltInParameterValue(sheet, BuiltInParameter.SHEET_CHECKED_BY, "SHEET_CHECKED_BY"));
            parameterInfos.Add(GetBuiltInParameterValue(sheet, BuiltInParameter.SHEET_DRAWN_BY, "SHEET_DRAWN_BY"));

            // Lấy giá trị Parameter theo tên TỪ SHEET (dựa vào code bạn cung cấp).
            parameterInfos.Add(GetParameterValueByName(sheet, "TKT_ISSUE_NUMBER_01_DATE", "TKT_ISSUE_NUMBER_01_DATE"));
            parameterInfos.Add(GetParameterValueByName(sheet, "TKT_TOTAL_REQUIRED", "TKT_TOTAL_REQUIRED"));


            return parameterInfos;
        }

        private ParameterInfo GetBuiltInParameterValue(Element element, BuiltInParameter bip, string parameterName)
        {
            Parameter parameter = element.get_Parameter(bip);
            if (parameter != null)
            {
                if (parameter.HasValue)
                    return new ParameterInfo { Name = parameterName, Value = ParameterToString(parameter) };
                else
                    return new ParameterInfo { Name = parameterName, Value = "Không có giá trị" };
            }
            return new ParameterInfo { Name = parameterName, Value = "Không tìm thấy parameter" };
        }

        private ParameterInfo GetParameterValueByName(Element element, string parameterName, string displayName)
        {
            Parameter parameter = element.LookupParameter(parameterName);

            if (parameter != null)
            {
                if (parameter.HasValue)
                    return new ParameterInfo { Name = displayName, Value = ParameterToString(parameter) };
                else
                    return new ParameterInfo { Name = displayName, Value = "Không có giá trị" };
            }

            return new ParameterInfo { Name = displayName, Value = "Không tìm thấy parameter" };
        }
        private string ParameterToString(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.String:
                    return parameter.AsString();
                case StorageType.Integer:
                    return parameter.AsInteger().ToString();
                case StorageType.Double:
                    return parameter.AsDouble().ToString();
                case StorageType.ElementId:
                    ElementId id = parameter.AsElementId();
                    if (id.IntegerValue >= 0)
                    {
                        return parameter.AsValueString(); // Hoặc có thể lấy tên của element: doc.GetElement(id).Name
                    }
                    else
                    {
                        return id.ToString();
                    }
                default:
                    return "Không thể hiển thị giá trị";
            }
        }
    }
}