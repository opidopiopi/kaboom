﻿using Kaboom.Domain.ShortcutActions;

namespace Kaboom.Testing.Mocks
{
    public class MockAction : Action
    {
        public int TriggerCount = 0;

        public MockAction(Shortcut shortcut) : base(shortcut)
        {
        }

        public override void Execute(IActionTarget actionTarget)
        {
            TriggerCount++;
        }

        public override string ToString()
        {
            return $"(MockAction: Shortcut:{Shortcut})";
        }
    }
}
