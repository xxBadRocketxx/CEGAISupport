using Autodesk.Revit.UI;
using CEGAISupport.Commands;
using CEGAISupport.Models;
using CEGAISupport.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents; // Cần thiết cho việc xử lý RichTextBox
using CEGAISupport.Utils;

namespace CEGAISupport
{
    public partial class RevitWindow : Window
    {
        private readonly UIApplication _uiApp;
        private readonly RevitCommand _revitCommand;
        private readonly GeminiService _geminiService;

        public RevitWindow(UIApplication uiApp, RevitCommand revitCommand)
        {
            InitializeComponent();

            _uiApp = uiApp;
            _revitCommand = revitCommand;
            _geminiService = new GeminiService();

            this.Loaded += RevitWindow_Loaded; // Đăng ký sự kiện Loaded
        }
        private void RevitWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UserInput.Focus(); // Focus vào ô nhập liệu khi cửa sổ được load
        }
        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add voice logic here.
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendUserMessage(); // Gọi hàm xử lý tin nhắn người dùng
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatHistoryGridBox.Document.Blocks.Clear(); // Xóa nội dung RichTextBox
            ClearButton.IsEnabled = false; // Disable nút Clear
        }

        private async Task SendUserMessage()
        {
            string userMessage = UserInput.Text; // Lấy tin nhắn từ ô nhập liệu

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return; // Không làm gì nếu tin nhắn trống
            }

            AddUserMessage(userMessage); // Hiển thị tin nhắn của người dùng

            UserInput.IsEnabled = false; // Disable ô nhập liệu
            AskButton.IsEnabled = false;  // Disable nút Ask
            ClearButton.IsEnabled = true; // Enable nút Clear

            // Gọi HandleCommand của RevitCommand để xử lý lệnh (bên trong có gọi Gemini nếu cần)
            await _revitCommand.HandleCommand(_uiApp, userMessage);

            UserInput.Text = string.Empty; // Xóa nội dung ô nhập liệu
        }

        // Phương thức để hiển thị tin nhắn từ AI (Gemini hoặc kết quả lệnh)
        public void AddAssistantMessage(string message)
        {
            Dispatcher.Invoke(() => // Đảm bảo cập nhật UI trên luồng UI chính
            {
                AddMessage(message, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3")), Brushes.Black, HorizontalAlignment.Left); // Màu xám nhạt
                UserInput.IsEnabled = true; // Enable lại ô nhập liệu
                AskButton.IsEnabled = true; // Enable lại nút Ask
            });
        }

        // Phương thức để hiển thị tin nhắn của người dùng
        private void AddUserMessage(string message)
        {
            AddMessage(message, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0E2FF")), Brushes.Black, HorizontalAlignment.Right); // Màu xanh nhạt
        }

        // Phương thức chung để thêm tin nhắn vào RichTextBox, xử lý màu và định dạng
        private void AddMessage(string message, Brush backgroundColor, Brush textColor, HorizontalAlignment alignment)
        {
            Paragraph paragraph = new Paragraph();  // Tạo một Paragraph mới
            paragraph.Margin = new Thickness(5);  // Đặt margin
            paragraph.Padding = new Thickness(5); // Đặt padding

            // Xử lý tag <red> để tô màu đỏ
            string[] parts = message.Split(new string[] { "<red>", "</red>" }, StringSplitOptions.None);
            bool isRed = false; // Biến để theo dõi trạng thái màu (đỏ hay không)

            foreach (string part in parts)
            {
                Run run = new Run(part); // Tạo một Run với nội dung là phần văn bản hiện tại
                run.Foreground = isRed ? Brushes.Red : textColor; // Đặt màu đỏ nếu isRed là true, ngược lại thì dùng màu mặc định
                paragraph.Inlines.Add(run); // Thêm Run vào Paragraph
                isRed = !isRed; // Đảo ngược trạng thái màu cho phần tiếp theo
            }

            paragraph.Background = backgroundColor; // Đặt màu nền cho Paragraph
            paragraph.TextAlignment = (TextAlignment)alignment; // Căn lề (trái hoặc phải)
            ChatHistoryGridBox.Document.Blocks.Add(paragraph); // Thêm Paragraph vào RichTextBox

            ChatHistoryGridBox.ScrollToEnd(); // Cuộn xuống cuối RichTextBox
        }

        // Xử lý sự kiện nhấn phím trong ô nhập liệu
        private async void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                // Nếu nhấn Shift + Enter: Thêm dòng mới
                int caretIndex = UserInput.CaretIndex;
                UserInput.Text = UserInput.Text.Insert(caretIndex, Environment.NewLine);
                UserInput.CaretIndex = caretIndex + Environment.NewLine.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                // Nếu chỉ nhấn Enter: Gửi tin nhắn
                e.Handled = true;
                await SendUserMessage();
            }
        }
        // Xử lý sự kiện nhấn nút lưu
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog(); // Mở hộp thoại lưu file
            dlg.FileName = "ChatLog"; // Tên file mặc định
            dlg.DefaultExt = ".txt"; // Phần mở rộng mặc định
            dlg.Filter = "Text documents (.txt)|*.txt"; // Bộ lọc file

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true) // Nếu người dùng chọn lưu
            {
                // Lấy toàn bộ nội dung của RichTextBox
                TextRange range = new TextRange(ChatHistoryGridBox.Document.ContentStart, ChatHistoryGridBox.Document.ContentEnd);

                try
                {
                    // Lưu nội dung vào file
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(dlg.FileName))
                    {
                        file.Write(range.Text);
                    }
                    MessageBox.Show("Chat log saved successfully!", "Save", MessageBoxButton.OK, MessageBoxImage.Information); // Thông báo thành công
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving chat log: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); // Thông báo lỗi
                }
            }
        }

    }
}