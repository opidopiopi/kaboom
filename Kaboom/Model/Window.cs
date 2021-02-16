using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class Window : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;
        private IWindowIdentity m_identity;
        private ITreeNode m_parent;

        public Window(IWindowIdentity identity, Rectangle bounds)
        {
            m_identity = identity;
            m_bounds = bounds;
        }

        public Rectangle Bounds
        {
            get => m_bounds;
            set => m_bounds = value;
        }

        public void Insert(ITreeNode child)
        {
            throw new InvalidChildForThisNode("You cannot insert any ITreeNodes under a Window. Windows are the leafs of the tree structure!");
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(ITreeNode parent)
        {
            m_parent = parent;
        }

        public ITreeNode GetParent()
        {
            return m_parent;
        }

        public List<ITreeNode> Children()
        {
            return new List<ITreeNode>();
        }
    }
}
