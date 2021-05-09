using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Domain
{
    public interface ISelection : IEntity
    {
        EntityID SelectedWindow { get; }

        void MoveSelectedWindow(Direction direction);
        void MoveSelection(Direction direction);
        void UnWrapSelectedWindow();
        void WrapSelectedWindow(Arrangement wrapper);
        void SelectWindow(EntityID windowID);
        void ClearSelection();
    }
}