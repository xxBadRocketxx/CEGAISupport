using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CEGAISupport.Commands.CommandHandlers
{
    public class ScheduleCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            // Tạo một ViewSchedule mới (ví dụ: schedule cho tất cả các cửa)
            // Đây chỉ là ví dụ đơn giản, bạn cần điều chỉnh để tạo schedule theo yêu cầu cụ thể.
            try
            {
                using (Transaction tx = new Transaction(doc, "Create Schedule"))
                {
                    tx.Start();

                    // Tạo schedule cho Category nào đó (ví dụ: Doors)
                    ViewSchedule schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_Doors));


                    // Thêm các trường (field) vào schedule (ví dụ: Family and Type, Mark, Level)
                    SchedulableField schedulableFieldFamilyAndType =
                        schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.GetName(doc) == "Family and Type");
                    //schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.ParameterId == new ElementId(BuiltInParameter.DOOR_TYPE_ID));
                    if (schedulableFieldFamilyAndType != null)
                    {
                        ScheduleField fieldFamilyAndType = schedule.Definition.AddField(schedulableFieldFamilyAndType);
                    }

                    SchedulableField schedulableFieldMark =
                         schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.GetName(doc) == "Mark");
                    //schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.ParameterId == new ElementId(BuiltInParameter.DOOR_NUMBER));
                    if (schedulableFieldMark != null)
                    {
                        ScheduleField fieldMark = schedule.Definition.AddField(schedulableFieldMark);
                    }

                    SchedulableField schedulableFieldLevel =
                        schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.GetName(doc) == "Level");
                    //schedule.Definition.GetSchedulableFields().FirstOrDefault(sf => sf.ParameterId == new ElementId(BuiltInParameter.LEVEL_PARAM));

                    if (schedulableFieldLevel != null)
                    {
                        ScheduleField fieldLevel = schedule.Definition.AddField(schedulableFieldLevel);
                    }

                    tx.Commit();
                    return "Schedule created successfully.  Check the Project Browser for the new schedule.";
                }
            }
            catch (Exception ex)
            {
                return $"Error creating schedule: {ex.Message}";
            }
        }
    }
}