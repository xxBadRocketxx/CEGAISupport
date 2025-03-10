using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CEGAISupport
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Tạo ribbon panel
            RibbonPanel panel = application.CreateRibbonPanel("CEGAI Support");

            // Lấy đường dẫn assembly
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Tạo PushButtonData
            PushButtonData buttonData = new PushButtonData(
                "RevitCommand",
                "CEGAI",
                assemblyPath,
                "CEGAISupport.Commands.RevitCommand"
            );

            // Thêm button vào panel
            PushButton button = panel.AddItem(buttonData) as PushButton;

            // Thêm tooltip (tùy chọn)
            button.ToolTip = "Interact with Gemini AI for Revit tasks.";

            // Thêm icon (tùy chọn)
            Uri imageUri = new Uri(Path.Combine(Path.GetDirectoryName(assemblyPath), "Resources", "icon.png")); // Đặt icon trong thư mục Resources
            BitmapImage image = new BitmapImage(imageUri);
            button.LargeImage = image;


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}