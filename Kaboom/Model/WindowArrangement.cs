using Kaboom.Abstract;

namespace Kaboom.Model
{
    public abstract class WindowArrangement : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;

        public WindowArrangement(Rectangle bounds)
        {
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
