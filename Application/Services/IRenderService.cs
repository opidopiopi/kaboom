using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;

namespace Kaboom.Application.Services
{
    public interface IRenderService : IVisitor
    {
        void ExecuteFromRoot(Arrangement rootArrangement);
        void HighlightWindow(Window selectedWindow);
    }
}