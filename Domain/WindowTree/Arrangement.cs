using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;
using System;
using System.Linq;

namespace Kaboom.Domain.WindowTree
{
    public abstract class Arrangement : BoundedTreeNode
    {
        private Axis[] m_supportedAxis;

        protected Arrangement(Axis[] supportedAxis)
        {
            m_supportedAxis = supportedAxis;
            Visible = true;
        }

        public void RemoveEmptyChildArrangements()
        {
            Children.RemoveAll(child =>
            {
                return child is Arrangement arrangement && arrangement.Children.Count == 0;
            });
        }

        public abstract void UpdateBoundsOfChildren();
        public abstract EntityID NeighbourOfChildInDirection(EntityID childID, Direction direction);

        public bool SupportsAxis(Axis axis)
        {
            return m_supportedAxis.Contains(axis);
        }

        public Window RemoveAndReturnWindow(EntityID childID)
        {
            var child = Children.Find(node => node.ID.Equals(childID));

            if (child is Window window)
            {
                Children.Remove(window);
                return window;
            }
            else
            {
                return null;
            }
        }

        public void WrapChildWithNode(EntityID childID, IBoundedTreeNode wrapper)
        {
            IBoundedTreeNode child;
            int index = RemoveChildAndReturnIndex(childID, out child);

            Children.Insert(index, wrapper);
            wrapper.InsertAsFirst(child);
        }

        public void UnWrapChildToSelf(EntityID childID)
        {
            IBoundedTreeNode child;
            int index = RemoveChildAndReturnIndex(childID, out child);

            if (child is Arrangement wrapper)
            {
                wrapper.Children.Reverse();

                foreach (var wrapperChild in wrapper.Children)
                {
                    Children.Insert(index, wrapperChild);
                }
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        private int RemoveChildAndReturnIndex(EntityID childID, out IBoundedTreeNode child)
        {
            child = FindChild(childID);

            if (child == null)
            {
                throw new Exception($"This node has no child with ID: {childID}!");
            }
            else
            {
                int index = Children.IndexOf(child);
                RemoveChild(childID);

                return index;
            }
        }

        public void PropagateVisibility() => Children.ForEach(child => child.Visible = this.Visible);
    }
}