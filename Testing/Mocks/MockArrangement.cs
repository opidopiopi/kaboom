using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Arrangements;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Mocks
{
    public class MockArrangement : Arrangement
    {
        private Axis[] m_supportedAxes;

        public MockArrangement(Axis[] axes)
        {
            this.m_supportedAxes = axes;
        }

        public override void MoveChild(ITreeNode child, Direction direction)
        {
            
        }

        public override ITreeNode NeighbourOfChildInDirection(ITreeNode treeNode, Direction direction)
        {
            return null;
        }

        public override bool SupportsAxis(Axis axis)
        {
            return m_supportedAxes.ToList().Contains(axis);
        }
    }
}
