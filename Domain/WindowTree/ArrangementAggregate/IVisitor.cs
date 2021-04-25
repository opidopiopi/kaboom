namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public interface IVisitor
    {
        void Visit(Arrangement arrangement);
        void Visit(Window window);
    }
}
