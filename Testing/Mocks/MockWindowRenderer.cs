using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockWindowRenderer : IRenderService
    {
        public List<Window> RenderedWindows = new List<Window>();

        public void HighlightWindow(Window selectedWindow)
        {

        }

        public void Render(Window window)
        {
            RenderedWindows.Add(window);
        }

        public void Render(Arrangement arrangement)
        {
            throw new System.NotImplementedException();
        }
    }
}
