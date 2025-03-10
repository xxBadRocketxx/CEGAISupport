namespace CEGAISupport.Models.RevitElements
{
    public class DoorData
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }    // Tên loại cửa (ví dụ: "Single-Flush: 36\" x 84\"")
        public double SillHeight { get; set; }
        public string Material { get; set; }
        public string UniqueId { get; set; }   // ID duy nhất của phần tử trong Revit
        public string HostWallUniqueId { get; set; }  // ID của tường chứa cửa
        public long Id { get; set; }
        // Thêm các thuộc tính khác nếu cần
    }
}