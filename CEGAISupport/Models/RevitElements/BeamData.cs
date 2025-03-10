namespace CEGAISupport.Models.RevitElements
{
    public class BeamData
    {
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public double Length { get; set; }
        public string MaterialName { get; set; }
        public double CrossSectionRotation { get; set; } // Góc xoay tiết diện
        public double Volume { get; set; }
        public string UniqueId { get; set; }
        public long Id { get; set; }
        // Thêm các thuộc tính khác...
    }
}