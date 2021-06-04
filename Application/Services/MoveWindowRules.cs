using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application.Services
{
    public static class MoveWindowRules
    {
        public static void MoveWindowByRules(EntityID windowID, Direction direction, Arrangement parent, IArrangementRepository arrangementRepository)
        {
            if (TryToMoveLocally(windowID, direction, ref parent))
            {
                return;
            }

            var window = parent.RemoveAndReturnWindow(windowID);
            if (TryToMoveUnderCurrentRoot(ref window, direction, ref parent, arrangementRepository))
            {
                return;
            }

            if (TryToMoveToDifferentRoot(ref window, direction, ref parent, arrangementRepository))
            {
                return;
            }

            Default(direction, parent, window);
        }

        private static bool TryToMoveLocally(EntityID windowID, Direction direction, ref Arrangement parent)
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

                return true;
            }
            else  //we can't move the child
            {
                return false;
            }
        }

        private static bool TryToMoveUnderCurrentRoot(ref Window window, Direction direction, ref Arrangement parent, IArrangementRepository arrangementRepository)
        {
            var superParent = arrangementRepository.FindParentOf(parent.ID);
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

                    return true;
                }

                parent = superParent;
                superParent = arrangementRepository.FindParentOf(parent.ID);
            }

            return false;
        }

        private static bool TryToMoveToDifferentRoot(ref Window window, Direction direction, ref Arrangement parent, IArrangementRepository arrangementRepository)
        {
            var otherRoot = arrangementRepository.FindNeighbourOfRoot(parent.ID, direction);
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

                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Default(Direction direction, Arrangement parent, Window window)
        {
            if (direction == Direction.Left || direction == Direction.Up)
            {
                parent.InsertAsFirst(window);
            }
            else
            {
                parent.InsertAsLast(window);
            }
        }
    }
}