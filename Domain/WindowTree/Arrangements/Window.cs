using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Arrangements;
using Kaboom.Domain.WindowTree.Exceptions;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree.Window
{
    public class Window : IHaveBounds, ITreeNode, IAcceptVisitors
    {
        public Rectangle Bounds { get => m_bounds; set => m_bounds = value; }
        public TreeNodeID ID { get; } = new TreeNodeID();
        public List<TreeNodeID> Children => new List<TreeNodeID>();


        private Rectangle m_bounds;

        public Window(Rectangle bounds)
        {
            m_bounds = bounds;
        }

        public void InsertAsFirst(TreeNodeID child)
        {
            throw new CannotInsertChild("You cannot insert a child under a window!");
        }

        public void InsertAsLast(TreeNodeID child)
        {
            throw new CannotInsertChild("You cannot insert a child under a window!");
        }

        public void Remove(TreeNodeID child)
        {
            throw new CannotRemoveChild("Windows have no childs, so you can't remove any!");
        }

        public bool IsLeaf()
        {
            return true;
        }

        public override string ToString()
        {
            return $"Window(ID: {ID}, Bounds: {m_bounds})";
        }

        public override bool Equals(object obj)
        {
            return obj is Window window &&
                   ID.Equals(window.ID);
        }

        public override int GetHashCode()
        {
            return -702609173 + ID.GetHashCode();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TreeNodeID FirstChild()
        {
            throw new System.NotImplementedException();
        }

        public TreeNodeID LastChild()
        {
            throw new System.NotImplementedException();
        }
    }
}
