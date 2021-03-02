using Kaboom.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public abstract class Arrangement : ITreeNode, IAcceptVisitors
    {
        private Guid m_guid = Guid.NewGuid();
        protected List<ITreeNode> m_children = new List<ITreeNode>();

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

        public abstract bool SupportsAxis(Axis axis);

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
