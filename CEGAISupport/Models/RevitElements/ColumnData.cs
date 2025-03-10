namespace CEGAISupport.Models.RevitElements
{
    public class ColumnData
    {
        public string FamilyName { get; set; }
        public string TypeName { get; set; }
        public double BaseLevel { get; set; } 
        public double TopLevel { get; set; }  
        public double BaseOffset { get; set; }
        public double TopOffset { get; set; }
        public double Length { get; set; }
        public string MaterialName { get; set; }
        public bool IsSlanted { get; set; }
        public double Volume { get; set; }
        public string UniqueId { get; set; }
        public long Id { get; set; }
        // Thêm các thuộc tính khác...
    }
}