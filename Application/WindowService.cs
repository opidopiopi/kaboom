using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application
{
    public class WindowService
    {
        private IArrangementRepository m_arrangements;
        private IWindowRenderer m_renderer;

        public WindowService(IArrangementRepository arrangements, IWindowRenderer renderer)
        {
            m_arrangements = arrangements;
            m_renderer = renderer;
        }

        public void InsertWindowIntoTree(Window newWindow)
        {
            var parent = m_arrangements.FindThatContainsPoint(newWindow.Bounds.X, newWindow.Bounds.Y);

            if (parent == null)
            {
                parent = m_arrangements.AnyRoot();
            }

            parent.InsertAsFirst(newWindow);
            UpdateTree(parent);
        }

        public void RemoveWindow(EntityID windowID)
        {
            var parent = m_arrangements.FindParentOf(windowID);

            if (parent == null) return;

            parent.RemoveChild(windowID);
            UpdateTree(parent);
        }

        public void MoveWindow(EntityID windowID, Direction direction)
        {
            var parent = m_arrangements.FindParentOf(windowID);

            if (parent == null)
            {
                throw new System.Exception($"invalid windowID: {windowID}!");
            }

            var neighbour = parent.FindChild(parent.NeighbourOfChildInDirection(windowID, direction));
            if (neighbour != null)  //we can move the window locally
            {
                if (neighbour.IsLeaf())
                {
                    parent.SwapChildren(windowID, neighbour.ID);
                }
                else
                {
                    if (direction == Direction.Left || direction == Direction.Up)
                    {
                        neighbour.InsertAsLast(parent.RemoveWindowAndReturn(windowID));
                    }
                    else
                    {
                        neighbour.InsertAsFirst(parent.RemoveWindowAndReturn(windowID));
                    }
                }

                UpdateTree(m_arrangements.FindParentOf(windowID));
            }
            else  //we need to move the window to a different arrangement
            {
                var window = parent.RemoveWindowAndReturn(windowID);

                var superParent = m_arrangements.FindParentOf(parent.ID);
                while (superParent != null) //go up the tree until we find an arrangement that allows us to move the window
                {
                    neighbour = parent.FindChild(superParent.NeighbourOfChildInDirection(parent.ID, direction));

                    if (neighbour != null)
                    {
                        if (direction == Direction.Left || direction == Direction.Up)
                        {
                            superParent.InsertBefore(window, parent);
                        }
                        else
                        {
                            superParent.InsertAfter(window, parent);
                        }

                        UpdateTree(superParent);
                        return;
                    }

                    parent = superParent;
                    superParent = m_arrangements.FindParentOf(parent.ID);
                }

                //when we don't find any arrangement just insert it into the tree again
                InsertWindowIntoTree(window);
            }
        }

        private void UpdateTree(Arrangement parent)
        {
            parent.UpdateBoundsOfChildren();
            parent.ForAllUnderlyingWindows((window) => m_renderer.Render(window));
        }
    }
}
