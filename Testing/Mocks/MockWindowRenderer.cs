using Kaboom.Application;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockWindowRenderer : IWindowRenderer
    {
        public List<Window> RenderedWindows = new List<Window>();
        public void Render(Window window)
        {
            RenderedWindows.Add(window);
        }
    }
}
