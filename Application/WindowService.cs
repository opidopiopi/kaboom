using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application
{
    public class WindowService : IWindowService
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
                    neighbour = superParent.FindChild(superParent.NeighbourOfChildInDirection(parent.ID, direction));

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

                throw new System.NotImplementedException("If this happenes we need to move the window to another root Arrangement");
            }
        }

        public EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected)
        {
            var parent = m_arrangements.FindParentOf(currentlySelected);

            if (parent == null)
            {
                throw new System.Exception("Somehow the currently selected entity has no parent....");
            }

            var nextSelected = parent.FindChild(parent.NeighbourOfChildInDirection(currentlySelected, direction));
            if (nextSelected is Window win)
            {
                return win.ID;
            }
            else if (nextSelected is Arrangement arr)
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    return arr.LastWindow();
                }
                else
                {
                    return arr.FirstWindow();
                }
            }

            var superParent = m_arrangements.FindParentOf(parent.ID);
            
            while(superParent != null)
            {
                var parentNeighbour = superParent.FindChild(superParent.NeighbourOfChildInDirection(parent.ID, direction));

                if(parentNeighbour is Window window)
                {
                    return window.ID;
                }
                else if (parentNeighbour is Arrangement arrangement)
                {
                    if (direction == Direction.Left || direction == Direction.Up)
                    {
                        return arrangement.LastWindow();
                    }
                    else
                    {
                        return arrangement.FirstWindow();
                    }
                }

                parent = superParent;
                superParent = m_arrangements.FindParentOf(parent.ID);
            }

            throw new System.NotImplementedException("If this happenes we need to check the other root arrangements");
        }

        private void UpdateTree(Arrangement parent)
        {
            parent.UpdateBoundsOfChildren();
            parent.ForAllUnderlyingWindows((window) => m_renderer.Render(window));
        }
    }
}
