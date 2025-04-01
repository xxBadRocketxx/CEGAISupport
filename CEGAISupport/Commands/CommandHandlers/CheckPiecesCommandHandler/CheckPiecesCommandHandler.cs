using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CEGAISupport.Models; // Đảm bảo using Models
using CEGAISupport.Commands.CommandHandlers.CheckPiecesCommandHandler;

namespace CEGAISupport.Commands.CommandHandlers.CheckPiecesCommandHandler
{
    public class CheckPiecesCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            // 1. Trích xuất tên Assembly (tương tự như CheckPartCommandHandler)
            List<string> potentialAssemblyNames = ExtractPotentialAssemblyNames(command);

            if (potentialAssemblyNames.Count == 0)
            {
                return "Please specify an assembly name containing digits (e.g., 'WPA001').";
            }

            string combinedResult = "";

            // 2. Duyệt qua các tên Assembly và xử lý
            foreach (string assemblyName in potentialAssemblyNames)
            {
                // 3. Tìm ViewSheet THEO TÊN CHÍNH XÁC
                ViewSheet foundSheet = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .FirstOrDefault(s => s.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase)); // Tìm chính xác

                if (foundSheet == null)
                {
                    combinedResult += $"Sheet '{assemblyName}' not found.\n";
                    continue;
                }

                // 4. Lấy thông tin Parameter từ Sheet
                AssemblyParameterLoader parameterLoader = new AssemblyParameterLoader();
                List<ParameterInfo> parameterInfos = parameterLoader.LoadSheetParameters(foundSheet);


                // 5. Định dạng đầu ra (sử dụng CheckPiecesOutputFormatter)
                CheckPiecesOutputFormatter formatter = new CheckPiecesOutputFormatter();
                string result = formatter.FormatOutput(parameterInfos, assemblyName, foundSheet); // Truyền thêm thông tin
                combinedResult += result;
                combinedResult += "-------\n";
            }

            return combinedResult;
        }

        // Hàm trích xuất tên Assembly (bao gồm cả dấu gạch nối)
        private List<string> ExtractPotentialAssemblyNames(string command)
        {
            // Regex mới: tìm các chuỗi có dạng chữ-số-chữ (bao gồm cả dấu gạch nối)
            MatchCollection matches = Regex.Matches(command, @"\b[a-zA-Z]+\d+(?:-\d+)?\b");
            List<string> result = new List<string>();
            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }
            return result;
        }

        // Hàm tìm Sheet dựa vào tên Assembly (tìm chính xác)
        private ViewSheet FindSheetByAssemblyName(Document doc, string assemblyName)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .FirstOrDefault(s => s.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase)); // Tìm chính xác
        }
    }
}
