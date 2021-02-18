using Kaboom.Abstract;
using System.Collections.Generic;

namespace Testing.Mocks
{
    public class MockTreeNodeWithBounds : ITreeNode, IHaveBounds
    {
        private List<ITreeNode> m_children = new List<ITreeNode>();
        private ITreeNode m_parent;
        private Rectangle m_bounds;

        public Rectangle Bounds
        {
            get => m_bounds;
            set => m_bounds = value;
        }

        public List<ITreeNode> Children()
        {
            return m_children;
        }

        public ITreeNode GetParent()
        {
            return m_parent;
        }

        public void Insert(ITreeNode child)
        {
            m_children.Add(child);
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            return m_children.Remove(child);
        }

        public void SetParent(ITreeNode parent)
        {
            m_parent = parent;
        }
    }
}
