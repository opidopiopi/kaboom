using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockArrangement : Arrangement
    {
        public MockArrangement(Axis[] axes)
            : base(axes)
        {
            
        }

        public override EntityID NeighbourOfChildInDirection(EntityID childID, Direction direction)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateBoundsOfChildren()
        {
            throw new System.NotImplementedException();
        }

        public List<IBoundedTreeNode> MyChildren => Children;
    }
}
