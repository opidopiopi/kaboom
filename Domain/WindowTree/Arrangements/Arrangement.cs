using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Exceptions;
using System;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public abstract class Arrangement : ITreeNode, IAcceptVisitors
    {
        protected List<ITreeNode> m_children = new List<ITreeNode>();
        private Guid m_guid = Guid.NewGuid();


        public abstract bool SupportsAxis(Axis axis);

        public abstract bool CanIMoveChild(Axis axis, Direction direction, ITreeNode child);

        public abstract void MoveChild(Axis axis, Direction direction, ITreeNode child);

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
            return false;
        }

        public void Remove(ITreeNode child)
        {
            m_children.Remove(child);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected void AssertSupportsAxisAndNodeIsDirectChild(Axis axis, Direction direction, ITreeNode node)
        {
            if (!SupportsAxis(axis))
            {
                throw new UnsupportedAxis($"This arrangement (this: {this}) does not support moving a child in axis: {axis}");
            }

            if (!m_children.Contains(node))
            {
                throw new GivenNodeIsNotADirectChild($"The given node {node} is not a direct child of this arrangement. (this: {this})");
            }
        }
    }
}
