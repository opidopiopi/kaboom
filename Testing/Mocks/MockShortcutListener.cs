using Kaboom.Domain.ShortcutActions;
using System;
using System.Collections.Generic;

namespace Kaboom.Testing.Mocks
{
    public class MockShortcutListener : IListenToShortcuts
    {
        public event EventHandler<ShortcutTriggeredEventArgs> ShortcutTriggered;
        public List<Shortcut> Shortcuts = new List<Shortcut>();

        public void RegisterShortcut(Shortcut shortcut)
        {
            Shortcuts.Add(shortcut);
        }

        public void UnRegisterShortcut(Shortcut shortcut)
        {
            Shortcuts.Remove(shortcut);
        }

        public void TriggerShortcut(Shortcut shortcut)
        {
            var args = new ShortcutTriggeredEventArgs();
            args.Shortcut = shortcut;

            ShortcutTriggered(this, args);
        }
    }
}
