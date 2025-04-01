using Autodesk.Revit.DB;
using CEGAISupport.Commands.CommandHandlers;
using System;
using System.Collections.Generic;


namespace CEGAISupport.Commands.Helpers
{
    public static class CommandParser
    {

        public static ICommandHandler ParseCommand(string command, Document doc, Dictionary<string, ICommandHandler> commandHandlerCache)
        {
            command = command.Trim().ToLower();

            // Ưu tiên tìm trong cache trước
            if (commandHandlerCache.TryGetValue(command, out ICommandHandler handler))
            {
                return handler;
            }

            // Lệnh "Check"
            if (command.Contains("check"))
            {
                handler = new CheckCommandHandler();
            }
            // Lệnh "Schedule"
            else if (command.Contains("schedule"))
            {
                handler = new ScheduleCommandHandler();
            }
            // Lệnh "Open"
            else if (command.Contains("open"))
            {
                handler = new OpenCommandHandler();
            }
            // Lệnh không hợp lệ
            else
            {
                handler = null;
            }

            // Lưu vào cache nếu tìm thấy handler
            if (handler != null)
            {
                commandHandlerCache[command] = handler;
            }

            return handler;

        }
    }
}