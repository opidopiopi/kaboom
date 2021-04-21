using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Domain.Services
{
    public interface IWindowService
    {
        void InsertWindowIntoTree(Window newWindow);
        void RemoveWindow(EntityID windowID);
        void MoveWindow(EntityID windowID, Direction direction);
        EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected);
        void UnWrapWindowParent(EntityID windowID);
        void HightlightWindow(EntityID windowID);
    }
}
