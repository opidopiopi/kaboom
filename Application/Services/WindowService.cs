using Kaboom.Domain;
using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;

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

        public void InsertWindowIntoTree(Window newWindow, ISelection selection)
        {
            var parent = m_arrangements.FindThatContainsPoint(newWindow.Bounds.X, newWindow.Bounds.Y);

            if (parent == null)
            {
                parent = m_arrangements.AnyRoot();
            }

            parent.InsertAsFirst(newWindow);
            UpdateTree();

            if(selection.SelectedWindow == null)
            {
                selection.SelectWindow(newWindow.ID);
            }
        }

        public void RemoveWindowFromTree(EntityID windowID, ISelection selection)
        {
            var parent = m_arrangements.FindParentOf(windowID);
            if (parent == null) return;

            parent.RemoveChild(windowID);

            UpdateTree();

            selection.ClearSelection();
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

            var window = parent.RemoveAndReturnWindow(windowID);
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

        public EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected)
        {
            var parent = m_arrangements.FindParentOf(currentlySelected);

            if (parent == null)
            {
                throw new System.Exception("Somehow the currently selected entity has no parent....");
            }

            var candidate = TryToGetNextLocally(direction, currentlySelected, ref parent);
            if (candidate != null)
            {
                return candidate;
            }

            Arrangement root;
            candidate = TryToGetNextUnderCurrentRoot(direction, parent, out root);
            if (candidate != null)
            {
                return candidate;
            }

            candidate = TryToGetNextFromDifferentRoot(direction, ref root);
            if (candidate != null)
            {
                return candidate;
            }

            return null;
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
                        neighbour.InsertAsLast(parent.RemoveAndReturnWindow(windowID));
                    }
                    else
                    {
                        neighbour.InsertAsFirst(parent.RemoveAndReturnWindow(windowID));
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
        private bool TryToMoveToDifferentRoot(ref Window window, Direction direction, ref Arrangement parent)
        {
            var otherRoot = m_arrangements.FindNeighbourOfRoot(parent.ID, direction);
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

        private EntityID TryToGetNextLocally(Direction direction, EntityID currentlySelected, ref Arrangement parent)
        {
            var neighbour = parent.FindChild(parent.NeighbourOfChildInDirection(currentlySelected, direction));
            if (neighbour is Window win)
            {
                return win.ID;
            }
            else if (neighbour is Arrangement arrangement)
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    return WindowFinder.LastWindowUnderArrangement(arrangement);
                }
                else
                {
                    return WindowFinder.FirstWindowUnderArrangement(arrangement);
                }
            }

            return null;
        }
        private EntityID TryToGetNextUnderCurrentRoot(Direction direction, Arrangement startParent, out Arrangement lastParent)
        {
            var superParent = m_arrangements.FindParentOf(startParent.ID);
            lastParent = startParent;

            while (superParent != null)
            {
                var res = TryToGetNextLocally(direction, lastParent.ID, ref superParent);
                if(res != null)
                {
                    return res;
                }

                lastParent = superParent;
                superParent = m_arrangements.FindParentOf(lastParent.ID);
            }

            return null;
        }
        private EntityID TryToGetNextFromDifferentRoot(Direction direction, ref Arrangement rootArrangement)
        {
            var rootNeighbour = m_arrangements.FindNeighbourOfRoot(rootArrangement.ID, direction);
            if (rootNeighbour != null)
            {
                if (direction == Direction.Left || direction == Direction.Up)
                {
                    return WindowFinder.LastWindowUnderArrangement(rootNeighbour);
                }
                else
                {
                    return WindowFinder.FirstWindowUnderArrangement(rootNeighbour);
                }
            }

            return null;
        }

        private void UpdateTree()
        {
            m_arrangements.RootArrangements().ForEach(rootID =>
            {
                var root = m_arrangements.Find(rootID);

                m_emptyArrangementRemover.ExecuteFromRoot(root);
                root.UpdateBoundsOfChildren();
                m_renderer.ExecuteFromRoot(root);
            });
        }
    }
}