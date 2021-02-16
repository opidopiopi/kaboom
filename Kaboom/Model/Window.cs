using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class Window : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;
        private IWindowIdentity m_identity;

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
            throw new System.NotImplementedException();
        }

        public void Remove(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(ITreeNode parent)
        {
            throw new System.NotImplementedException();
        }

        public ITreeNode GetParent()
        {
            throw new System.NotImplementedException();
        }
    }
}
