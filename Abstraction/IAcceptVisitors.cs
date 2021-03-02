namespace Kaboom.Abstraction
{
    public interface IAcceptVisitors
    {
        void Accept(IVisitor visitor);
    }
}
