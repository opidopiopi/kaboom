using System;

namespace Kaboom.Domain.ShortcutActions
{
    public class ShortcutTriggeredEventArgs : EventArgs
    {
        public Shortcut Shortcut { get; set; }
    }


    public interface IListenToShortcuts
    {
        event EventHandler<ShortcutTriggeredEventArgs> ShortcutTriggered;

        void RegisterShortcut(Shortcut shortcut);
        void UnRegisterShortcut(Shortcut shortcut);
    }
}
