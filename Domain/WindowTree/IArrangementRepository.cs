using Kaboom.Domain.WindowTree.ValueObjects;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree
{
    public interface IArrangementRepository
    {
        void InsertRoot(Arrangement arrangement);
        void RemoveRoot(EntityID arrangementID);
        List<Arrangement> RootArrangements();
        Arrangement AnyRoot();

        Arrangement Find(EntityID arrangementID);
        Arrangement FindThatContainsPoint(int x, int y);
        Arrangement FindParentOf(EntityID arrangementOrWindow);

        Arrangement FindNeighbourOfRoot(EntityID arrangementID, Direction direction);
    }
}