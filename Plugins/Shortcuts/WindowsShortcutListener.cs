using Kaboom.Application.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plugins.Shortcuts
{
    [ExcludeFromCodeCoverage]
    public class WindowsShortcutListener : IListenToShortcuts, IActionEventSource
    {
        private HashSet<IActionEventListener> m_eventListeners = new HashSet<IActionEventListener>();

        public WindowsShortcutListener()
        {
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyPressed);
        }

        private void HotKeyPressed(object sender, HotKeyEventArgs args)
        {
            var shortcut = new Shortcut(args.Modifiers, args.Key);

            Console.WriteLine($"[ShortcutListener]     Shortcut pressed: {shortcut}");
            m_eventListeners.ToList().ForEach(listener => listener.OnActionEvent(shortcut));
        }

        public void RegisterShortcut(Shortcut shortcut)
        {
            Console.WriteLine($"[ShortcutListener]     New Shortcut: {shortcut}");
            HotKeyManager.RegisterHotKey(shortcut.Key, shortcut.Modifier);
        }

        public void AddActionEventListener(IActionEventListener actionEventListener)
        {
            m_eventListeners.Add(actionEventListener);
        }
    }
}