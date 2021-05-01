using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application.Services
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
