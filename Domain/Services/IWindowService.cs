using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Domain.Services
{
    public interface IWindowService
    {
        void InsertWindowIntoTree(Window newWindow, ISelection selection);
        void RemoveWindowFromTree(EntityID windowID, ISelection selection);
        void MoveWindow(EntityID windowID, Direction direction);
        EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected);
        void UnWrapWindowParent(EntityID windowID);
        void HightlightWindow(EntityID windowID);
    }
}