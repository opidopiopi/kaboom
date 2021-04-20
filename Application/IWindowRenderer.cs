using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application
{
    public interface IWindowRenderer
    {
        void Render(Window window);
        void HighlightWindow(Window selectedWindow);
    }
}
