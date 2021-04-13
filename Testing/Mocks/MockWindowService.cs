using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockWindowService : IWindowService
    {
        public List<Window> Windows = new List<Window>();

        public void InsertWindowIntoTree(Window newWindow)
        {
            Windows.Add(newWindow);
        }

        public void MoveWindow(EntityID windowID, Direction direction)
        {

        }

        public EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected)
        {
            return Windows[0].ID;
        }

        public void RemoveWindow(EntityID windowID)
        {
            Windows.RemoveAll(window => window.ID.Equals(windowID));
        }
    }
}
