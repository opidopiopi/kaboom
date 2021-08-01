using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using System.Collections.Generic;

namespace Kaboom.Application.Services
{
    public interface IRenderService : IVisitor
    {
        void RenderTrees(IEnumerable<Arrangement> rootArrangements);
        void HighlightWindow(Window selectedWindow);
    }
}