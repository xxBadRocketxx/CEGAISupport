using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using CEGAISupport.Models; // Thêm dòng này

namespace CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler
{
    public class FamilyParameterLoader
    {
        public List<ParameterInfo> LoadFamilyParameters(Document doc, string familyName)
        {
            if (string.IsNullOrWhiteSpace(familyName))
            {
                return new List<ParameterInfo>();
            }

            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol));

            List<FamilySymbol> matchingFamilySymbols = collector.Cast<FamilySymbol>()
                .Where(fs => fs.FamilyName.IndexOf(familyName, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (matchingFamilySymbols.Count == 0)
            {
                return new List<ParameterInfo>();
            }

            HashSet<string> parameterNamesToShow = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "IDENTITY_DESCRIPTION",
                "CONTROL_MARK",
                "desc_b",
                "desc_g",
                "mark_b",
                "mark_g"
            };

            List<ParameterInfo> parameterInfos = new List<ParameterInfo>();

            foreach (FamilySymbol familySymbol in matchingFamilySymbols)
            {
                ParameterSet parameters = familySymbol.Parameters;

                foreach (Parameter param in parameters)
                {
                    if (parameterNamesToShow.Contains(param.Definition.Name))
                    {
                        string paramValue = GetParameterValue(param, doc);
                        parameterInfos.Add(new ParameterInfo { Name = $"{familySymbol.FamilyName} - {param.Definition.Name}", Value = paramValue });
                    }
                }
            }
            return parameterInfos;
        }

        private string GetParameterValue(Parameter param, Document doc)
        {
            switch (param.StorageType)
            {
                case StorageType.Double:
                    return param.AsDouble().ToString();
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.String:
                    return param.AsString();
                case StorageType.ElementId:
                    ElementId id = param.AsElementId();
                    if (id.IntegerValue >= 0)
                    {
                        return doc.GetElement(id).Name;
                    }
                    else
                    {
                        return id.ToString();
                    }
                default:
                    return "unsupported parameter type";
            }
        }
    }
    // Xóa class ParameterInfo ở đây.
}