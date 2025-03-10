using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input; // Cho KeyDown event

namespace CEGAISupport.Views
{
    public partial class RevitWindow : Window
    {
        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();
        private System.Windows.Threading.DispatcherTimer _timer;
        private DateTime _startTime;

        public RevitWindow()
        {
            InitializeComponent();
            ChatHistoryListBox.ItemsSource = ChatMessages;  // Liên kết dữ liệu
                                                            // Khởi tạo timer
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Cập nhật thời gian
            TimeSpan elapsed = DateTime.Now - _startTime;
            TimerTextBlock.Text = $"{(int)elapsed.TotalSeconds}s";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy prompt từ TextBox
            string prompt = PromptTextBox.Text;

            // Kiểm tra prompt khác rỗng
            if (!string.IsNullOrWhiteSpace(prompt))
            {
                // Thêm tin nhắn của người dùng vào lịch sử
                ChatMessages.Add(new ChatMessage { Message = prompt, BackgroundColor = "#cce5ff", ForegroundColor = "Black" }); // Màu xanh nhạt

                // Xóa nội dung TextBox
                PromptTextBox.Text = "";

                // Focus lại vào TextBox để người dùng nhập tiếp
                PromptTextBox.Focus();
                // Disable nút Ask và Clear
                AskButton.IsEnabled = false;

                // Bắt đầu計時
                _startTime = DateTime.Now;
                _timer.Start();
            }
            // Gọi phương thức xử lý ở RevitCommand (sẽ được thực hiện thông qua ExternalEvent)
            DialogResult = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Xóa lịch sử trò chuyện
            ChatMessages.Clear();

            // Xóa nội dung TextBox
            PromptTextBox.Text = string.Empty;

            // Enable lại nút Ask
            AskButton.IsEnabled = true;
            // Dừng và reset timer
            _timer.Stop();
            TimerTextBlock.Text = "0s";
        }

        private void PromptTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) // Giữ Shift và Enter để xuống dòng
            {
                // Thêm xuống dòng vào TextBox
                int caretIndex = PromptTextBox.CaretIndex;
                PromptTextBox.Text = PromptTextBox.Text.Insert(caretIndex, Environment.NewLine);
                PromptTextBox.CaretIndex = caretIndex + Environment.NewLine.Length; // Di chuyển con trỏ xuống dòng
                e.Handled = true; // Ngăn không cho xử lý Enter mặc định
            }
            else if (e.Key == Key.Enter)
            {
                // Xử lý như nút Send
                SendButton_Click(sender, e);
                e.Handled = true; // Ngăn không cho xử lý Enter mặc định (thêm xuống dòng)
            }
        }
    }

    // Class cho tin nhắn trong lịch sử trò chuyện
    public class ChatMessage : INotifyPropertyChanged
    {
        private string _message;
        private string _backgroundColor;
        private string _foregroundColor;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public string ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                OnPropertyChanged("ForegroundColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}