using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Exceptions;
using Kaboom.Domain.WindowTree.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public abstract class Arrangement : ITreeNode, IAcceptVisitors
    {
        private List<ITreeNode> m_children = new List<ITreeNode>();
        private Guid m_guid = Guid.NewGuid();

        public List<ITreeNode> Children { get => m_children; }

        public abstract bool SupportsAxis(Axis axis);

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

        public Arrangement FindParentOf(ITreeNode aChild)
        {
            if (m_children.Contains(aChild))
            {
                return this;
            }
            else
            {
                foreach (var child in m_children.Where(c => !c.IsLeaf()).Select(c => (Arrangement)c))
                {
                    var res = child.FindParentOf(aChild);

                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            return null;
        }

        public abstract void MoveChild(ITreeNode child, Direction direction);

        public abstract ITreeNode NeighbourOfChildInDirection(ITreeNode treeNode, Direction direction);

        protected void AssertSupportsAxisAndNodeIsDirectChild(Axis axis, ITreeNode node)
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

        public ITreeNode FirstChild() => m_children.First();

        public ITreeNode LastChild() => m_children.Last();
    }
}
