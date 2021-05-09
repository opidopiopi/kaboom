using Kaboom.Domain.WindowTree.Helpers;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeLeaf : BoundedTreeLeaf
    {
        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
