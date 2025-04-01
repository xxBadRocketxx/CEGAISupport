using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

namespace CEGAISupport
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Tạo Ribbon Tab mới (quan trọng)
            application.CreateRibbonTab("Model Tools");

            // Tạo Ribbon Panel (bên trong Tab mới)
            RibbonPanel panel = application.CreateRibbonPanel("Model Tools", "CEGAI Support"); // Thêm tên tab

            // Đường dẫn đến assembly (DLL) của add-in
            string assemblyPath = typeof(App).Assembly.Location;

            // Tạo PushButtonData
            PushButtonData buttonData = new PushButtonData(
                "CEGAIChatBot", // Tên nội bộ
                "ChatBot", // Tên hiển thị
                assemblyPath, // Đường dẫn assembly
                "CEGAISupport.Commands.RevitCommand" // Tên lớp Command
            );

            // Thêm nút vào Ribbon Panel
            PushButton button = panel.AddItem(buttonData) as PushButton;

            // Thêm icon cho nút
            try
            {
                Uri uriImage = new Uri(@"C:\_chatbot\images\logo_ceg_32x32.png"); // Đường dẫn tuyệt đối, bạn nên thay đổi cho phù hợp
                BitmapImage largeImage = new BitmapImage(uriImage);
                button.LargeImage = largeImage;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Lỗi", "Không thể load icon: " + ex.Message);
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}