namespace CEGAISupport.Models.RevitElements
{
    public class WindowData
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public double SillHeight { get; set; }
        public string MaterialName { get; set; }
        public string UniqueId { get; set; }
        public string HostWallUniqueId { get; set; } // ID của tường
        public long Id { get; set; }
        // Thêm các thuộc tính khác...
    }
}