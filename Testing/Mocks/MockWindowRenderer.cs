using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Mocks
{
    public class MockWindowRenderer : IRenderService
    {
        public List<Window> RenderedWindows = new List<Window>();

        public void HighlightWindow(Window selectedWindow)
        {

        }

        public void RenderTrees(IEnumerable<Arrangement> rootArrangements)
        {
            rootArrangements.ToList().ForEach(root => root.Accept(this));
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
