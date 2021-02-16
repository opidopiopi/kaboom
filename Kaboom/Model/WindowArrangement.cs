﻿using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public abstract class WindowArrangement : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;
        private ITreeNode m_parent;
        private List<ITreeNode> m_children = new List<ITreeNode>();

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
            m_children.Add(child);
            child.SetParent(this);
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
    }
}