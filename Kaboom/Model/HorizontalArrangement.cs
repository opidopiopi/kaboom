using Kaboom.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Model
{
    public class HorizontalArrangement : WindowArrangement
    {
        protected override void UpdateBoundsOfChildren()
        {
            if (m_children.Count == 0) return;

            List<IHaveBounds> children = m_children.Cast<IHaveBounds>().ToList();   //I don't like

            int numberOfChildren = children.Count();
            int widthPerChild = Bounds.Width / numberOfChildren;

            for(int i = 0; i < numberOfChildren; i++)
            {
                children[i].Bounds = new Rectangle(Bounds.X + i * widthPerChild, Bounds.Y, widthPerChild, Bounds.Height);
            }
        }

        public override void MoveChildLeft(ITreeNode child, ITreeNode caller)
        {
            if(child == caller)  //we want to move our direct child
            {
                int index = m_children.IndexOf(child);

                if (index > 0)
                {
                    if (m_children[index - 1].IsLeaf())
                    {
                        m_children.Insert(index - 1, child);
                    }
                    else
                    {
                        m_children[index - 1].Insert(child);
                    }
                    m_children.Remove(child);
                    UpdateBoundsOfChildren();

                    return;
                }
            }
            else  //we want to move callers child
            {
                int index = m_children.IndexOf(caller);

                if (index > 0)
                {
                    if(m_children[index - 1].IsLeaf())
                    {
                        m_children.Insert(index, child);
                    }
                    else
                    {
                        m_children[index - 1].Insert(child);
                    }
                    return;
                }
            }

            base.MoveChildLeft(child, this);
        }

        public override void MoveChildRight(ITreeNode child, ITreeNode caller)
        {
            if (child == caller)  //we want to move our direct child
            {
                int index = m_children.IndexOf(child);

                if (index < m_children.Count - 1)
                {
                    if (m_children[index + 1].IsLeaf())
                    {
                        m_children.Insert(index + 2, child);
                    }
                    else
                    {
                        m_children[index + 1].Insert(child);
                    }
                    m_children.Remove(child);
                    UpdateBoundsOfChildren();

                    return;
                }
            }
            else  //we want to move callers child
            {
                int index = m_children.IndexOf(caller);

                if (index < m_children.Count - 1)
                {
                    if (m_children[index + 1].IsLeaf())
                    {
                        m_children.Insert(index + 1, child);
                    }
                    else
                    {
                        m_children[index + 1].Insert(child);
                    }
                    return;
                }
            }
            base.MoveChildRight(child, this);
        }
    }
}
