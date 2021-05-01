using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application
{
    public interface ISelection
    {
        EntityID SelectedWindow { get; }

        void InsertWindow(Window window);
        void MoveSelectedWindow(Direction direction);
        void MoveSelection(Direction direction);
        void RemoveWindow(EntityID windowID);
        void UnWrapSelectedWindow();
        void WrapSelectedWindow(Arrangement wrapper);
        void SelectWindow(EntityID windowID);
    }
}