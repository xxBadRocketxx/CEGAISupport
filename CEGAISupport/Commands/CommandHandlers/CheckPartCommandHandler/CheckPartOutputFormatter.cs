using System.Collections.Generic;
using CEGAISupport.Commands.CommandHandlers;
using CEGAISupport.Models;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class CheckPartOutputFormatter
    {
        // Phương thức format đầu ra, nhận Dictionary<TextNoteInfo, bool> thay vì List<TextNoteInfo>
        public string FormatOutput(List<ParameterInfo> parameterInfos, List<string> parentFamilies, Dictionary<TextNoteInfo, bool> textNotes)
        {
            string result = "";

            // Format thông tin Parameter
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

            // Format thông tin Parent Family
            if (parentFamilies.Count > 0)
            {
                result += "\nParent Families:\n";
                foreach (var familyName in parentFamilies)
                {
                    result += $"{familyName}\n";
                }
            }
            else
            {
                result += "\nNo parent families found.\n";
            }

            // Format thông tin TextNote và thêm đánh dấu màu đỏ nếu cần
            if (textNotes.Count > 0)
            {
                result += "\nText Notes:\n";
                foreach (var kvp in textNotes) // Duyệt qua Dictionary
                {
                    TextNoteInfo textNote = kvp.Key; // Lấy TextNoteInfo
                    bool isMatch = kvp.Value;      // Lấy kết quả so sánh

                    // Thêm tag <red> nếu không khớp (isMatch == false)
                    string formattedText = isMatch ? textNote.Text : $"<red>{textNote.Text}</red>";
                    result += $"View: {textNote.ViewName}, Sheet: {textNote.SheetName}, \nText: {formattedText}\n"; // Dùng formattedText
                    result += "-----------\n";
                }
            }
            else
            {
                result += "\nNo related text notes found in views.";
            }

            return result;
        }
    }
}