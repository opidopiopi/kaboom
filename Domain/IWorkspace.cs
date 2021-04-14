using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application
{
    public interface IWorkspace
    {
        EntityID SelectedWindow { get; }

        void InsertWindow(Window window);
        void MoveSelectedWindow(Direction direction);
        void MoveSelection(Direction direction);
        void RemoveWindow(EntityID windowID);
        void UnWrapSelectedWindow();
        void WrapSelectedWindow(Arrangement wrapper);
    }
}