using Autodesk.Revit.DB;
using System;
using CEGAISupport.Utils; // Để dùng StringExtensions

namespace CEGAISupport.Commands.CommandHandlers
{
    public class OpenCommandHandler : ICommandHandler
    {
        public string Execute(string command, Document doc)
        {
            // Kiểm tra xem lệnh là "open view" hay "open sheet"
            if (command.ContainsCaseInsensitive("view"))
            {
                OpenViewCommandHandler viewHandler = new OpenViewCommandHandler();
                return viewHandler.Execute(command, doc);
            }
            else if (command.ContainsCaseInsensitive("sheet"))
            {
                OpenSheetCommandHandler sheetHandler = new OpenSheetCommandHandler();
                return sheetHandler.Execute(command, doc);
            }
            else
            {
                return "Please specify whether to open a 'view' or a 'sheet'.";
            }
        }
    }
}