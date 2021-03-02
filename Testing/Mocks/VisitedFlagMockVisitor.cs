using Kaboom.Abstraction;

namespace Kaboom.Testing.Mocks
{
    public class VisitedFlagMockVisitor : IVisitor
    {
        private bool m_visited = false;

        public bool HasBeenVisited { get => m_visited; }

        public void Visit(object acceptor)
        {
            m_visited = true;
        }
    }
}
