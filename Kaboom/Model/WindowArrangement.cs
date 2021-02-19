using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public abstract class WindowArrangement : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;
        private ITreeNode m_parent;
        protected List<ITreeNode> m_children = new List<ITreeNode>();

        public Rectangle Bounds
        {
            get => m_bounds;
            set
            {
                m_bounds = value;
                UpdateBoundsOfChildren();
            }
        }

        public void Insert(ITreeNode child)
        {
            m_children.Add(child);
            child.SetParent(this);

            UpdateBoundsOfChildren();
        }

        public bool RemoveAndReturnSuccess(ITreeNode toBeRemoved)
        {
            foreach(var child in m_children)
            {
                if(child == toBeRemoved)
                {
                    return m_children.Remove(child);
                }
                else if (child.RemoveAndReturnSuccess(toBeRemoved))
                {
                    return true;
                }
            }
            return false;
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
            return m_children;
        }

        protected abstract void UpdateBoundsOfChildren();
    }
}
