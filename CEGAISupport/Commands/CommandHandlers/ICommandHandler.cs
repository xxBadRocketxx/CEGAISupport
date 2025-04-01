using Autodesk.Revit.DB;

namespace CEGAISupport.Commands.CommandHandlers
{
    public interface ICommandHandler
    {
        string Execute(string command, Document doc);
    }
}