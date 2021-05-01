namespace Kaboom.Abstraction
{
    public interface IVisitor<VisitedType>
    {
        void Visit(VisitedType toBeVisited);
    }

    public interface ICanBeVisited<VisitedType>
    {
        void Accept(IVisitor<VisitedType> visitor);
    }

    public interface IHaveChildrenThatCanBeVisited<VisitedType>
    {
        void VisitAllChildren(IVisitor<VisitedType> visitor);
    }

    public class DepthFirstVisitor<TypeOfVisited> : IVisitor<TypeOfVisited>
        where TypeOfVisited : ICanBeVisited<TypeOfVisited>, IHaveChildrenThatCanBeVisited<TypeOfVisited>
    {
        private IVisitor<TypeOfVisited> m_visitor;

        public DepthFirstVisitor(IVisitor<TypeOfVisited> visitor)
        {
            m_visitor = visitor;
        }

        public void Visit(TypeOfVisited toBeVisited)
        {
            toBeVisited.VisitAllChildren(this);
            toBeVisited.Accept(m_visitor);
        }
    }
}