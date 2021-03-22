using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Arrangements;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeNodeWithBounds : ITreeNode, IHaveBounds
    {
        private List<TreeNodeID> m_children = new List<TreeNodeID>();
        private Rectangle m_bounds;
        private bool m_isLeaf;

        public TreeNodeID ID { get; } = new TreeNodeID();
        List<TreeNodeID> ITreeNode.Children => m_children;

        public MockTreeNodeWithBounds(bool isLeaf = true)
        {
            m_isLeaf = isLeaf;
        }

        public Rectangle Bounds
        {
            get => m_bounds;
            set => m_bounds = value;
        }

        public void InsertAsFirst(TreeNodeID child)
        {
            m_children.Insert(0, child);
        }

        public void InsertAsLast(TreeNodeID child)
        {
            m_children.Add(child);
        }

        public bool IsLeaf()
        {
            return m_isLeaf;
        }

        public TreeNodeID FirstChild()
        {
            return m_children.Count > 0 ? m_children[0] : null;
        }

        public TreeNodeID LastChild()
        {
            return m_children.Count > 0 ? m_children[m_children.Count - 1] : null;
        }

        public void Remove(TreeNodeID child)
        {
            m_children.Remove(child);
        }
    }
}
