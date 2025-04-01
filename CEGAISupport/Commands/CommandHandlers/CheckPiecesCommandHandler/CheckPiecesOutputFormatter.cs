using System.Collections.Generic;
using CEGAISupport.Models;
using Autodesk.Revit.DB;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class CheckPiecesOutputFormatter
    {
        public string FormatOutput(List<ParameterInfo> parameterInfos, string assemblyName, ViewSheet sheet)
        {
            string result = "";
            result += $"Assembly: {assemblyName.ToUpper()}\n";
            result += $"Sheet: {sheet.SheetNumber} - {sheet.Name}\n";

            if (parameterInfos.Count > 0)
            {
                result += "Parameters:\n";
                foreach (var paramInfo in parameterInfos)
                {
                    result += $"{paramInfo.Name}: {paramInfo.Value}\n";
                }
            }
            else
            {
                result += "No matching parameters found.\n";
            }

            return result;
        }
    }
}