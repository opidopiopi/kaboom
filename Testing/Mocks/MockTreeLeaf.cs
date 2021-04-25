using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Testing.Mock
{
    public class MockTreeLeaf : BoundedTreeLeaf
    {
        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
