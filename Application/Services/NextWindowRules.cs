using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application.Services
{
    public static class NextWindowRules
    {
        public static EntityID NextWindowByRules(Direction direction, EntityID currentlySelected, ref Arrangement parent, IArrangementRepository arrangementRepository)
        {
            var candidate = TryLocally(direction, currentlySelected, ref parent);
            if (candidate != null)
            {
                return candidate;
            }

            Arrangement currentRoot;
            candidate = TryUnderCurrentRoot(direction, parent, out currentRoot, arrangementRepository);
            if (candidate != null)
            {
                return candidate;
            }

            candidate = TryFromDifferentRoot(direction, ref currentRoot, arrangementRepository);
            if (candidate != null)
            {
                return candidate;
            }

            return null;
        }

        private static EntityID TryLocally(Direction direction, EntityID currentlySelected, ref Arrangement parent)
        {
            var neighbour = parent.FindChild(parent.NeighbourOfChildInDirection(currentlySelected, direction));
            if (neighbour is Window win)
            {
                return win.ID;
            }
            else if (neighbour is Arrangement arrangement)
            {
                return FirstWindowInDirection(direction, arrangement);
            }

            return null;
        }

        private static EntityID TryUnderCurrentRoot(Direction direction, Arrangement startParent, out Arrangement lastParent, IArrangementRepository arrangementRepository)
        {
            var superParent = arrangementRepository.FindParentOf(startParent.ID);
            lastParent = startParent;

            while (superParent != null)
            {
                var res = TryLocally(direction, lastParent.ID, ref superParent);
                if (res != null)
                {
                    return res;
                }

                lastParent = superParent;
                superParent = arrangementRepository.FindParentOf(lastParent.ID);
            }

            return null;
        }

        private static EntityID TryFromDifferentRoot(Direction direction, ref Arrangement rootArrangement, IArrangementRepository arrangementRepository)
        {
            var rootNeighbour = arrangementRepository.FindNeighbourOfRoot(rootArrangement.ID, direction);
            if (rootNeighbour != null)
            {
                return FirstWindowInDirection(direction, rootNeighbour);
            }

            return null;
        }

        private static EntityID FirstWindowInDirection(Direction direction, Arrangement arrangement)
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
    }
}