using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockWindowRenderer : IRenderService
    {
        public List<Window> RenderedWindows = new List<Window>();

        public void ExecuteFromRoot(Arrangement rootArrangement)
        {
            rootArrangement.Accept(this);
        }

        public void HighlightWindow(Window selectedWindow)
        {

        }

        public void Visit(Arrangement arrangement)
        {
            arrangement.VisitAllChildren(this);
        }

        public void Visit(Window window)
        {
            RenderedWindows.Add(window);
        }
    }
}
