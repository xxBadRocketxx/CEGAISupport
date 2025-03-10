// Tạo file này nếu chưa có
using Autodesk.Revit.UI;
using System.Threading.Tasks;

namespace CEGAISupport.Commands.CommandHandlers
{
    public interface ICommandHandler
    {
        Task HandleAsync(string prompt, UIDocument uiDoc);
    }
}