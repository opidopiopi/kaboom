using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class Screen : IHaveBounds, ITreeNode
    {
        private Rectangle m_bounds;
        private Workspace m_workspace;
        private WindowArrangement m_rootArrangement;

        public Screen(Rectangle bounds)
        {
            m_bounds = bounds;
        }

        public Rectangle Bounds
        { 
            get => m_bounds;
            set => m_bounds = value;
        }

        public void Insert(WindowArrangement arrangement)
        {
            if(m_rootArrangement == null)
            {
                m_rootArrangement = arrangement;
                m_rootArrangement.SetParent(this);
                m_rootArrangement.Bounds = Bounds;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        public void Insert(Window window)
        {
            m_rootArrangement.Insert(window);
        }

        public void Insert(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            //note that you can't remove the root arrangement
            return m_rootArrangement.RemoveAndReturnSuccess(child);
        }

        public void SetParent(Workspace workspace)
        {
            m_workspace = workspace;
        }

        public void SetParent(ITreeNode parent)
        {
            throw new ParentMustBeWorkspace(
                        string.Format(
                            "You are trying to set the parent of a screen to an object of type: {0}, only type Workspace is allowed!",
                            parent.GetType())
                        );
        }

        public ITreeNode GetParent()
        {
            return m_workspace;
        }

        public List<ITreeNode> Children()
        {
            return new List<ITreeNode>() { m_rootArrangement };
        }
    }
}
