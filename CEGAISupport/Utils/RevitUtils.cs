using Autodesk.Revit.DB;
using CEGAISupport.Models.RevitElements;
using System.Collections.Generic;
using System.Linq;

namespace CEGAISupport.Utils
{
    public static class RevitUtils
    {
        public static List<WallData> GetWallDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .Select(wall => GetWallData(wall, doc)) // Truyền doc vào đây
                .ToList();
        }
        public static List<DoorData> GetDoorDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
               .OfCategory(BuiltInCategory.OST_Doors)
               .WhereElementIsNotElementType()
               .Cast<FamilyInstance>()
               .Select(door => GetDoorData(door, doc)) // Truyền doc vào đây
               .ToList();

        }

        public static WallData GetWallData(Wall wall, Document doc) // Thêm Document doc
        {
            return new WallData
            {
                Length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble(),
                Height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble(),
                Thickness = wall.Width,
                MaterialName = wall.WallType.get_Parameter(BuiltInParameter.MATERIAL_NAME).AsString(),
                BaseOffset = wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble(),
                TopOffset = wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).AsDouble(),
                WallType = wall.WallType.Name,
                Volume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(),
                Area = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(),
                LocationLine = (wall.Location as LocationCurve)?.Curve.ToString(),
                UniqueId = wall.UniqueId,
                Id = wall.Id.IntegerValue,

            };
        }

        public static DoorData GetDoorData(FamilyInstance door, Document doc) // Thêm Document doc
        {
            return new DoorData
            {
                Width = door.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsDouble(),
                Height = door.get_Parameter(BuiltInParameter.DOOR_HEIGHT).AsDouble(),
                FamilyName = door.Symbol.FamilyName,
                TypeName = door.Symbol.Name,
                SillHeight = door.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).AsDouble(),
                //Material = door.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM).AsValueString(), // Lấy vật liệu
                UniqueId = door.UniqueId,
                HostWallUniqueId = (door.Host as Wall)?.UniqueId,
                Id = door.Id.IntegerValue,
            };
        }
        // Phương thức lấy thông tin dự án (loại bỏ area)
        public static (string, string, string) GetProjectInfo(Document doc)
        {
            ProjectInfo projectInfo = doc.ProjectInformation;
            return (projectInfo.Name, projectInfo.Address, projectInfo.Number);
        }

        public static List<ColumnData> GetColumnDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns) // Hoặc OST_Columns
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .Select(column => GetColumnData(column, doc)) // Truyền doc vào GetColumnData
                .ToList();
        }

        public static ColumnData GetColumnData(FamilyInstance column, Document doc) // Thêm Document doc
        {
            bool isStructural = column.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns;
            // Sử dụng Level.Elevation thay vì cố gắng cast trực tiếp
            Level baseLevel = doc.GetElement(column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).AsElementId()) as Level;
            Level topLevel = doc.GetElement(column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).AsElementId()) as Level;

            return new ColumnData
            {
                FamilyName = column.Symbol.FamilyName,
                TypeName = column.Symbol.Name,
                BaseLevel = baseLevel?.Elevation ?? 0,  // Sử dụng toán tử null-conditional
                TopLevel = topLevel?.Elevation ?? 0,    // Sử dụng toán tử null-conditional
                BaseOffset = column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).AsDouble(),
                TopOffset = column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).AsDouble(),
                Length = isStructural ? column.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble() : column.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble(),
                MaterialName = column.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM)?.AsValueString(),
                IsSlanted = column.get_Parameter(BuiltInParameter.SLANTED_COLUMN_TYPE_PARAM).AsInteger() != 0,
                Volume = column.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED)?.AsDouble() ?? 0,
                UniqueId = column.UniqueId,
                Id = column.Id.IntegerValue,
            };
        }


        public static List<BeamData> GetBeamDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralFraming)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .Select(beam => GetBeamData(beam, doc)) // Truyền doc
                .ToList();
        }

        public static BeamData GetBeamData(FamilyInstance beam, Document doc) // Thêm doc
        {
            return new BeamData
            {
                FamilyName = beam.Symbol.FamilyName,
                TypeName = beam.Symbol.Name,
                Length = beam.get_Parameter(BuiltInParameter.INSTANCE_LENGTH_PARAM).AsDouble(),
                MaterialName = beam.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM)?.AsValueString(),
                CrossSectionRotation = beam.get_Parameter(BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE)?.AsDouble() ?? 0,
                Volume = beam.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED)?.AsDouble() ?? 0,
                UniqueId = beam.UniqueId,
                Id = beam.Id.IntegerValue
            };
        }
        public static List<FloorData> GetFloorDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Floor))
                .Cast<Floor>()
                .Select(floor => GetFloorData(floor, doc)) // Truyền doc
                .ToList();
        }

        public static FloorData GetFloorData(Floor floor, Document doc) // Thêm doc
        {

            return new FloorData
            {
                FloorType = floor.FloorType.Name,
                Thickness = floor.FloorType.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM).AsDouble(),
                Area = floor.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(),
                Volume = floor.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(),
                MaterialName = floor.FloorType.get_Parameter(BuiltInParameter.MATERIAL_NAME)?.AsString(),
                UniqueId = floor.UniqueId,
                Id = floor.Id.IntegerValue,
            };
        }

        public static List<WindowData> GetWindowDataList(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Windows)
                .WhereElementIsNotElementType()
                .Cast<FamilyInstance>()
                .Select(window => GetWindowData(window, doc)) // Truyền doc
                .ToList();
        }
        public static WindowData GetWindowData(FamilyInstance window, Document doc) // Thêm doc
        {
            return new WindowData
            {
                Width = window.get_Parameter(BuiltInParameter.WINDOW_WIDTH).AsDouble(),
                Height = window.get_Parameter(BuiltInParameter.WINDOW_HEIGHT).AsDouble(),
                FamilyName = window.Symbol.FamilyName,
                TypeName = window.Symbol.Name,
                SillHeight = window.get_Parameter(BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM).AsDouble(),
                //MaterialName = window.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM)?.AsValueString(),
                UniqueId = window.UniqueId,
                HostWallUniqueId = (window.Host as Wall)?.UniqueId,
                Id = window.Id.IntegerValue,
            };
        }
    }
}