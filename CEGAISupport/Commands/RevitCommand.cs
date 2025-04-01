using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using CEGAISupport.Commands.CommandHandlers; // Đảm bảo có using này
using CEGAISupport.Commands.Helpers;
using System.Windows;
using CEGAISupport.Services; // Thêm services
using System.Threading.Tasks;
using System;

namespace CEGAISupport.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        private RevitWindow _revitWindow; // Thêm biến để giữ tham chiếu
        private static readonly System.Collections.Generic.Dictionary<string, ICommandHandler> _commandHandlerCache = new System.Collections.Generic.Dictionary<string, ICommandHandler>(System.StringComparer.OrdinalIgnoreCase);
        private GeminiService _geminiService = new GeminiService();
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            if (_revitWindow == null)
            {
                _revitWindow = new RevitWindow(uiApp, this); //Truyền UIApplication vào
                _revitWindow.Closed += RevitWindow_Closed;
            }
            //_revitWindow = new RevitWindow(commandData, this); // Truyền commandData vào constructor.
            _revitWindow.Show();

            return Result.Succeeded;
        }
        private void RevitWindow_Closed(object sender, System.EventArgs e)
        {
            _revitWindow = null; // Giải phóng tham chiếu khi cửa sổ đóng.
        }
        //Thêm phương thức xử lý cho RevitWindows
        public async Task HandleCommand(UIApplication uiApp, string userInput)
        {
            Document doc = uiApp.ActiveUIDocument.Document;
            // Phân tích lệnh và lấy Command Handler
            ICommandHandler handler = CommandParser.ParseCommand(userInput, doc, _commandHandlerCache);

            if (handler != null)
            {
                try
                {
                    // Thực thi lệnh
                    string resultMessage = handler.Execute(userInput, doc);
                    _revitWindow.AddAssistantMessage(resultMessage); // Hiển thị kết quả
                }
                catch (Exception ex)
                {
                    _revitWindow.AddAssistantMessage($"Error executing command: {ex.Message}"); // Hiển thị lỗi
                }
            }
            else
            {
                // Không tìm thấy handler -> Gọi Gemini AI
                try
                {
                    string geminiResponse = await _geminiService.SendMessage(userInput);
                    _revitWindow.AddAssistantMessage(geminiResponse); // Hiển thị phản hồi từ Gemini
                }
                catch (Exception ex)
                {
                    _revitWindow.AddAssistantMessage($"Error from Gemini AI: {ex.Message}");
                }
            }
        }
    }
}