using Kaboom.Domain.WindowTree.Arrangements;
using System.Linq;

namespace Kaboom.Testing.Mocks
{
    public class MockArrangement : Arrangement
    {
        private Axis[] m_supportedAxes;

        public MockArrangement(ITreeNodeRepository treeNodeRepository, Axis[] axes)
            : base(treeNodeRepository)
        {
            this.m_supportedAxes = axes;
        }

        public override bool SupportsAxis(Axis axis)
        {
            return m_supportedAxes.ToList().Contains(axis);
        }

        public override bool CanIMoveChild(Axis axis, Direction direction, TreeNodeID child)
        {
            return true;
        }

        public override void MoveChild(Axis axis, Direction direction, TreeNodeID child)
        {
            throw new System.NotImplementedException();
        }
    }
}
