using Autodesk.Revit.DB;
using System;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class CheckCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            // Chuẩn hóa chuỗi lệnh: chữ thường, loại bỏ khoảng trắng thừa
            command = command.Trim().ToLower();

            // Kiểm tra xem lệnh có chứa "part", "parts", "piece", hoặc "pieces" không
            if (command.Contains("part") || command.Contains("parts"))
            {
                // Nếu có "part" hoặc "parts", chuyển hướng đến CheckPartCommandHandler
                // Sử dụng tên đầy đủ của class:
                var partHandler = new CEGAISupport.Commands.CommandHandlers.CheckPartCommandHandler.CheckPartCommandHandler();
                return partHandler.Execute(command, doc);
            }
            else if (command.Contains("piece") || command.Contains("pieces"))
            {
                // Nếu có "piece" hoặc "pieces", chuyển hướng đến CheckPiecesCommandHandler
                // Sử dụng tên đầy đủ của class:
                var piecesHandler = new CEGAISupport.Commands.CommandHandlers.CheckPiecesCommandHandler.CheckPiecesCommandHandler();
                return piecesHandler.Execute(command, doc);
            }
            else
            {
                // Nếu không có các từ khóa trên, hiển thị thông báo hỏi
                return "What type do you want to check? (part or piece or sheet)";
            }
        }
    }
}