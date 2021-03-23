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

        public override bool SupportsAxis(Axis axis)
        {
            return m_supportedAxes.ToList().Contains(axis);
        }

        public List<ITreeNode> Children()
        {
            return m_children;
        }

        public override bool CanIMoveChild(Axis axis, Direction direction, ITreeNode child)
        {
            return true;
        }

        public override void MoveChild(Axis axis, Direction direction, ITreeNode child)
        {
            throw new System.NotImplementedException();
        }
    }
}
