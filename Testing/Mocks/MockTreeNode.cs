using Kaboom.Domain.WindowTree.Helpers;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeNode : BoundedTreeNode
    {
        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
