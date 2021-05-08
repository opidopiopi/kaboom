namespace Kaboom.Domain.WindowTree.Helpers
{
    public interface IVisitor
    {
        void Visit(Arrangement arrangement);
        void Visit(Window window);
    }
}