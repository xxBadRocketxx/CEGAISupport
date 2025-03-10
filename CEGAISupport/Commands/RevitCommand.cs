using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CEGAISupport.Commands.CommandHandlers;
using CEGAISupport.Services;
using CEGAISupport.Views;
using System.Threading.Tasks;
using System;

namespace CEGAISupport.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        private GeminiService _geminiService;
        private ExternalEvent _externalEvent;
        private string _prompt;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;

            // Khởi tạo Gemini Service (Đưa API key vào file cấu hình sau)
            _geminiService = new GeminiService("YOUR_API_KEY");

            // Mở form
            RevitWindow window = new RevitWindow();
            if (window.ShowDialog() == true)
            {
                _prompt = window.PromptTextBox.Text;
                _externalEvent = ExternalEvent.Create(new RevitEventHandler(this, uiDoc));
                _externalEvent.Raise();
            }

            return Result.Succeeded;
        }

        private async Task ProcessCommand(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            if (string.IsNullOrEmpty(_prompt)) return;

            // Phân loại và xử lý lệnh
            ICommandHandler handler = null;

            // Sử dụng IndexOf và StringComparison (đã sửa)
            if (_prompt.IndexOf("how many", StringComparison.OrdinalIgnoreCase) >= 0 ||
                _prompt.IndexOf("đếm", StringComparison.OrdinalIgnoreCase) >= 0 ||
                _prompt.IndexOf("count", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler = new CountCommandHandler(_geminiService); // Truyền GeminiService
            }
            else if (_prompt.IndexOf("pick", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     _prompt.IndexOf("select", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     _prompt.IndexOf("chọn", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler = new PickCommandHandler(); // Không cần GeminiService
            }
            else if (_prompt.IndexOf("scope box", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler = new ScopeBoxCommandHandler(); // Không cần GeminiService
            }
            else if (_prompt.IndexOf("thống kê", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     _prompt.IndexOf("schedule", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler = new ScheduleCommandHandler(_geminiService); // Truyền _geminiService (đã sửa)
            }
            else if (_prompt.IndexOf("ID", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                handler = new IDCommandHandler();  // Không cần
            }
            // ... thêm các trường hợp khác ...

            if (handler != null)
            {
                await handler.HandleAsync(_prompt, uiDoc);
            }
            else
            {
                // Xử lý các lệnh chung với Gemini (nếu có, phần này bạn sẽ triển khai sau)
            }
        }
        public class RevitEventHandler : IExternalEventHandler
        {
            private RevitCommand _command;
            private UIDocument _uiDoc;

            public RevitEventHandler(RevitCommand command, UIDocument uiDoc)
            {
                _command = command;
                _uiDoc = uiDoc;
            }

            public void Execute(UIApplication app)
            {
                Task.Run(async () => { await _command.ProcessCommand(_uiDoc); });
            }

            public string GetName() => "CEGAISupport Revit Event Handler";
        }
    }
}