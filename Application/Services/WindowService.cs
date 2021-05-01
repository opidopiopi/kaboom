using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application.Services
{
    public class WindowService : IWindowService
    {
        private IArrangementRepository m_arrangements;
        private IRenderService m_renderer;
        private EmptyArrangementRemover m_emptyArrangementRemover = new EmptyArrangementRemover();

        public WindowService(IArrangementRepository arrangements, IRenderService renderer)
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
            UpdateTree();
        }

        public void RemoveWindow(EntityID windowID)
        {
            var parent = m_arrangements.FindParentOf(windowID);
            if (parent == null) return;

            parent.RemoveChild(windowID);

            UpdateTree();
        }

        public void MoveWindow(EntityID windowID, Direction direction)
        {
            var parent = m_arrangements.FindParentOf(windowID);

            if (parent == null)
            {
                throw new System.Exception($"invalid windowID: {windowID}!");
            }

            if (TryToMoveLocally(windowID, direction, ref parent))
            {
                return;
            }

            var window = parent.RemoveWindowAndReturn(windowID);

            if (TryToMoveUnderCurrentRoot(ref window, direction, ref parent))
            {
                UpdateTree();
                return;
            }

            if (TryToMoveToDifferentRoot(ref window, direction, ref parent))
            {
                UpdateTree();
                return;
            }
            else
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    parent.InsertAsFirst(window);
                }
                else
                {
                    parent.InsertAsLast(window);
                }

                UpdateTree();
                return;
            }
        }

        private bool TryToMoveToDifferentRoot(ref Window window, Direction direction, ref Arrangement parent)
        {
            var otherRoot = m_arrangements.FindNeighbourOfRootInDirection(parent.ID, direction);
            if (otherRoot != null)
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    otherRoot.InsertAsLast(window);
                }
                else
                {
                    otherRoot.InsertAsFirst(window);
                }

                UpdateTree();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryToMoveUnderCurrentRoot(ref Window window, Direction direction, ref Arrangement parent)
        {
            var superParent = m_arrangements.FindParentOf(parent.ID);
            while (superParent != null) //go up the tree until we find an arrangement that allows us to move the window
            {
                if (superParent.SupportsAxis(direction.Axis))
                {
                    if (direction == Direction.Left || direction == Direction.Up)
                    {
                        superParent.InsertBefore(window, parent);
                    }
                    else
                    {
                        superParent.InsertAfter(window, parent);
                    }

                    UpdateTree();
                    return true;
                }

                parent = superParent;
                superParent = m_arrangements.FindParentOf(parent.ID);
            }

            return false;
        }

        private bool TryToMoveLocally(EntityID windowID, Direction direction, ref Arrangement parent)
        {
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

                UpdateTree();
                return true;
            }
            else  //we can't move the child
            {
                return false;
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

            while (superParent != null)
            {
                var parentNeighbour = superParent.FindChild(superParent.NeighbourOfChildInDirection(parent.ID, direction));

                if (parentNeighbour is Window window)
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

            var rootNeighbour = m_arrangements.FindNeighbourOfRootInDirection(parent.ID, direction);
            if (rootNeighbour != null)
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    return rootNeighbour.LastWindow();
                }
                else
                {
                    return rootNeighbour.FirstWindow();
                }
            }
            else
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    return parent.FirstWindow();
                }
                else
                {
                    return parent.LastWindow();
                }
            }
        }

        public void UnWrapWindowParent(EntityID windowID)
        {
            var parent = m_arrangements.FindParentOf(windowID);
            var superParent = m_arrangements.FindParentOf(parent.ID);

            if (superParent != null)
            {
                superParent.UnWrapChildToSelf(parent.ID);
                UpdateTree();
            }
        }

        public void HightlightWindow(EntityID windowID)
        {
            if (windowID != null)
            {
                m_renderer.HighlightWindow(m_arrangements.FindParentOf(windowID).FindChild(windowID) as Window);
            }
        }

        private void UpdateTree()
        {
            m_arrangements.RootArrangements().ForEach(arrangementID =>
            {
                var root = m_arrangements.Find(arrangementID);

                m_emptyArrangementRemover.ExecuteFromRoot(root);
                root.UpdateBoundsOfChildren();
                root.ForAllUnderlyingWindows((window) => m_renderer.Render(window));
            });
        }
    }
}