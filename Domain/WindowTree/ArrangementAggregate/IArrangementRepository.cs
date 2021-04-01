using Kaboom.Abstraction;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public interface IArrangementRepository
    {
        void InsertRoot(Arrangement arrangement);
        void RemoveRoot(EntityID arrangementID);
        List<EntityID> RootArrangements();
        Arrangement AnyRoot();

        Arrangement Find(EntityID arrangementID);
        Arrangement FindThatContainsPoint(int x, int y);
        Arrangement FindParentOf(EntityID arrangementOrWindow);
    }
}
