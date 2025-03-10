namespace CEGAISupport.Models.RevitElements
{
    public class WallData
    {
        public double Length { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }
        public string MaterialName { get; set; }
        public double BaseOffset { get; set; }
        public double TopOffset { get; set; }
        public string WallType { get; set; }      // Tên loại tường (ví dụ: "Generic - 8\"")
        public double Volume { get; set; }      // Thể tích
        public double Area { get; set; }       // Diện tích
        public string LocationLine { get; set; } // Ví dụ: "Wall Centerline"
        public string UniqueId { get; set; } // ID duy nhất của phần tử trong Revit
        public long Id { get; set; } // ID

        // Thêm các thuộc tính khác nếu cần (ví dụ: tọa độ, thông số, ...)
    }
}