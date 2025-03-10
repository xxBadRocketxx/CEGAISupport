// Trong Models/RevitModel.cs
using CEGAISupport.Models.RevitElements;
using System.Collections.Generic;

namespace CEGAISupport.Models
{
    public class RevitModel
    {
        public List<WallData> Walls { get; set; }
        public List<DoorData> Doors { get; set; }
        public List<ColumnData> Columns { get; set; }
        public List<BeamData> Beams { get; set; }
        public List<FloorData> Floors { get; set; }
        public List<WindowData> Windows { get; set; }
        public string ProjectInfo { get; set; }
        public string ProjectAddress { get; set; }
        public string ProjectName { get; set; }
        public double ProjectArea { get; set; }
    }
}