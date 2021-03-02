using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Exceptions;

namespace Kaboom.Domain.WindowTree.Window
{
    public class Window : IHaveBounds, ITreeNode, IAcceptVisitors
    {
        private WindowIdentity m_identity = new WindowIdentity();
        private Rectangle m_bounds;

        public Window(Rectangle bounds)
        {
            m_bounds = bounds;
        }

        public Rectangle Bounds { get => m_bounds; set => m_bounds = value; }
        public WindowIdentity Identity { get => m_identity; }

        public void InsertAsFirst(ITreeNode child)
        {
            throw new CannotInsertChild("You cannot insert a child under a window!");
        }

        public void InsertAsLast(ITreeNode child)
        {
            throw new CannotInsertChild("You cannot insert a child under a window!");
        }

        public void Remove(ITreeNode child)
        {
            throw new CannotRemoveChild("Windows have no childs, so you can't remove any!");
        }

        public bool IsLeaf()
        {
            return true;
        }

        public override string ToString()
        {
            return $"Window(ID: {Identity}, Bounds: {m_bounds})";
        }

        public override bool Equals(object obj)
        {
            return obj is Window window &&
                   Identity.Equals(window.Identity);
        }

        public override int GetHashCode()
        {
            return -702609173 + Identity.GetHashCode();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
