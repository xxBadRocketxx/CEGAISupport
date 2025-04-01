using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Thêm using cho Regex
// Đảm bảo using đúng các namespace của các lớp hỗ trợ
using CEGAISupport.Utils;
using CEGAISupport.Models;
using System.Linq;
using CEGAISupport.Commands.CommandHandlers;
using CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler;

namespace CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler
{
    public class CheckPartCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            // 1. Tìm tất cả các chuỗi con chứa chữ số
            List<string> potentialFamilyNames = ExtractPotentialFamilyNames(command);

            if (potentialFamilyNames.Count == 0)
            {
                return "Please specify a family name containing digits (e.g., 'EM001').";
            }

            // Chuỗi tổng hợp kết quả cho tất cả family
            string combinedResult = "";

            // 2. Duyệt qua các chuỗi con, kiểm tra và xử lý
            foreach (string familyName in potentialFamilyNames)
            {
                // Sử dụng các lớp hỗ trợ để thực hiện các phần việc
                FamilyParameterLoader parameterLoader = new FamilyParameterLoader();
                List<ParameterInfo> parameterInfos = parameterLoader.LoadFamilyParameters(doc, familyName);

                ParentFamilyLoader parentFamilyLoader = new ParentFamilyLoader();
                List<string> parentFamilies = parentFamilyLoader.LoadParentFamilies(doc, familyName);

                TextNoteSearchCommandHandler textNoteHandler = new TextNoteSearchCommandHandler();
                // SỬA Ở ĐÂY: Nhận Dictionary<TextNoteInfo, bool>
                Dictionary<TextNoteInfo, bool> textNotes = textNoteHandler.FindTextNotes(doc, familyName, "");

                // Định dạng đầu ra: SỬA Ở ĐÂY - truyền Dictionary
                CheckPartOutputFormatter formatter = new CheckPartOutputFormatter();
                string result = formatter.FormatOutput(parameterInfos, parentFamilies, textNotes);
                combinedResult += result; // Cộng dồn kết quả
            }
            return combinedResult; // Trả về chuỗi tổng hợp
        }
        // Hàm mới: Tìm tất cả các chuỗi con chứa chữ số
        private List<string> ExtractPotentialFamilyNames(string command)
        {
            // Sử dụng Regex để tìm các chuỗi con có chứa ít nhất một chữ số
            MatchCollection matches = Regex.Matches(command, @"\b[a-zA-Z]*\d+[a-zA-Z0-9]*\b");  // \b: word boundary, \d+: một hoặc nhiều chữ số
            List<string> result = new List<string>();
            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }
            return result;
        }
    }
}