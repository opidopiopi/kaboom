using Kaboom.Abstraction;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeNodeWithBounds : ITreeNode, IHaveBounds
    {
        private List<ITreeNode> m_children = new List<ITreeNode>();
        private Rectangle m_bounds;
        private bool m_isLeaf;

        public MockTreeNodeWithBounds(bool isLeaf = true)
        {
            m_isLeaf = isLeaf;
        }

        public Rectangle Bounds
        {
            get => m_bounds;
            set => m_bounds = value;
        }

        public List<ITreeNode> Children()
        {
            return m_children;
        }

        public void InsertAsFirst(ITreeNode child)
        {
            m_children.Insert(0, child);
        }

        public void InsertAsLast(ITreeNode child)
        {
            m_children.Add(child);
        }

        public bool IsLeaf()
        {
            return m_isLeaf;
        }

        public void Remove(ITreeNode child)
        {
            m_children.Remove(child);
        }
    }
}
