using Kaboom.Application.Actions;
using System;

namespace Kaboom.Testing.Mocks
{
    public class MockActionEvent : IActionEvent
    {
        private Guid m_guid = Guid.NewGuid();

        public bool Equals(IActionEvent actionEvent)
        {
            return (actionEvent is MockActionEvent mockEvent) && mockEvent.m_guid.Equals(m_guid);
        }
    }
}