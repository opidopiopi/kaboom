using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application.Services
{
    public interface IRenderService
    {
        void Render(Arrangement arrangement);
        void Render(Window window);
        void HighlightWindow(Window selectedWindow);
    }
}
