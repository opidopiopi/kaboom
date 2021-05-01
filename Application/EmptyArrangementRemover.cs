using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application
{
    public class EmptyArrangementRemover : IVisitor
    {
        public void ExecuteFromRoot(Arrangement rootArrangement)
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