using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class Window : IHaveBounds, ITreeNode, ICanBeMoved
    {
        private Rectangle m_bounds;
        private IWindowIdentity m_identity;
        private WindowArrangement m_parent;
        private ISetWindowBounds m_windowBoundsSetter;

        public Window(IWindowIdentity identity, Rectangle bounds, ISetWindowBounds windowBoundsSetter)
        {
            m_identity = identity;
            m_bounds = bounds;
            m_windowBoundsSetter = windowBoundsSetter;
        }

        public Rectangle Bounds
        {
            get => m_bounds;
            set
            {
                m_bounds = value;
                m_windowBoundsSetter.SetBoundsOfWindowWithIdentity(m_identity, Bounds);
            }
        }

        public void Insert(ITreeNode child)
        {
            throw new InvalidChildForThisNode("You cannot insert any ITreeNodes under a Window. Windows are the leafs of the tree structure!");
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            return false;   //has no children so we can't remove any
        }

        public void SetParent(ITreeNode parent)
        {
            m_parent = (WindowArrangement) parent;
        }

        public ITreeNode GetParent()
        {
            return m_parent;
        }

        public List<ITreeNode> Children()
        {
            return new List<ITreeNode>();
        }

        public IWindowIdentity Identity()
        {
            return m_identity;
        }

        public void MoveUp()
        {
            m_parent.MoveChildUp(this, this);
        }

        public void MoveDown()
        {
            m_parent.MoveChildDown(this, this);
        }

        public void MoveLeft()
        {
            m_parent.MoveChildLeft(this, this);
        }

        public void MoveRight()
        {
            m_parent.MoveChildRight(this, this);
        }

        public bool IsLeaf()
        {
            return true;
        }
    }
}
