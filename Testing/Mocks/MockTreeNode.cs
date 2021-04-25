using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Testing.Mock
{
    public class MockTreeNode : BoundedTreeNode
    {
        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
