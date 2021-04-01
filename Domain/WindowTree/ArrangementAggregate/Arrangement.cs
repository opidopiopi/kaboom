﻿using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.General;
using System;
using System.Linq;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public delegate void WindowCallback(Window window);

    public abstract class Arrangement : BoundedTreeNode
    {
        private Axis[] m_supportedAxis;

        protected Arrangement(Axis[] supportedAxis)
        {
            m_supportedAxis = supportedAxis;
        }

        public abstract void UpdateBoundsOfChildren();
        public abstract EntityID NeighbourOfChildInDirection(EntityID childID, Direction direction);
        public void SwapChildren(EntityID childA, EntityID childB)
        {
            var tempA = FindChild(childA);
            var tempB = FindChild(childB);

            var indexA = Children.IndexOf(tempA);
            var indexB = Children.IndexOf(tempB);

            Children[indexA] = tempB;
            Children[indexB] = tempA;
        }

        public bool SupportsAxis(Axis axis)
        {
            return m_supportedAxis.Contains(axis);
        }

        public void RemoveChild(EntityID childID)
        {
            Children.RemoveAll(node => node.ID.Equals(childID));
        }

        public Window RemoveWindowAndReturn(EntityID childID)
        {
            var child = Children.Find(node => node.ID.Equals(childID));

            if (child is Window window)
            {
                Children.Remove(child);
                return window;
            }
            else
            {
                return null;
            }
        }

        public void WrapChildWithNode(EntityID childID, IBoundedTreeNode substitute)
        {
            var child = FindChild(childID);

            if (child == null)
            {
                throw new Exception($"This node has no child with ID: {childID}!");
            }
            else
            {
                int index = Children.IndexOf(child);
                RemoveChild(childID);

                Children.Insert(index, substitute);
                substitute.InsertAsFirst(child);
            }
        }

        public void UnWrapChildToSelf(EntityID childID)
        {
            var child = FindChild(childID);

            if (child == null)
            {
                throw new Exception($"This node has no child with ID: {childID}!");
            }
            else
            {
                int index = Children.IndexOf(child);
                RemoveChild(childID);

                var next = child.LastChild();
                while (next != null)
                {
                    child.Remove(next);
                    Children.Insert(index, next);

                    next = child.LastChild();
                }
            }
        }

        public IBoundedTreeNode FindChild(EntityID childID)
        {
            return Children.Find(node => node.ID.Equals(childID));
        }

        public void ForAllUnderlyingWindows(WindowCallback callback)
        {
            Children.ForEach((child) => {
                if (child is Window window)
                {
                    callback(window);
                }
                else if (child is Arrangement arrangement)
                {
                    arrangement.ForAllUnderlyingWindows(callback);
                }
            });
        }

        public Arrangement FindParentOf(EntityID arrangementOrWindow)
        {
            if (FindChild(arrangementOrWindow) != null)
            {
                return this;
            }
            else
            {
                foreach (var child in Children)
                {
                    if(child is Arrangement arrangement)
                    {
                        var res = arrangement.FindParentOf(arrangementOrWindow);

                        if (res != null)
                        {
                            return res;
                        }
                    }
                }
            }
            return null;
        }
    }
}
