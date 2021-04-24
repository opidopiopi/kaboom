using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application
{
    public class RemoveEmptyArrangements : IVisitor
    {
        public void RemoveEmptyArrangementsFromTree(Arrangement rootArrangement)
        {
            Visit(rootArrangement);
        }

        public void Visit(Arrangement arrangement)
        {
            arrangement.VisitAllChildren(this);
            arrangement.RemoveEmptyChildArrangements();
        }

        public void Visit(Window window)
        {
            //do nothing
        }
    }
}