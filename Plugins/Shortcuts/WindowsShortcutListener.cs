using Kaboom.Application.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plugins.Shortcuts
{
    [ExcludeFromCodeCoverage]
    public class WindowsShortcutListener : IListenToShortcuts, IActionEventSource, IDisposable
    {
        private HashSet<IActionEventListener> eventListeners = new HashSet<IActionEventListener>();
        private EventHandler<HotKeyEventArgs> eventHandler;

        public WindowsShortcutListener()
        {
            eventHandler = new EventHandler<HotKeyEventArgs>(HotKeyPressed);
            HotKeyManager.HotKeyPressed += eventHandler;
        }

        private void HotKeyPressed(object sender, HotKeyEventArgs args)
        {
            var shortcut = new Shortcut(args.Modifiers, args.Key);

            Console.WriteLine($"[ShortcutListener]     Shortcut pressed: {shortcut}");
            eventListeners.ToList().ForEach(listener => listener.OnActionEvent(shortcut));
        }

        public void RegisterShortcut(Shortcut shortcut)
        {
            Console.WriteLine($"[ShortcutListener]     New Shortcut: {shortcut}");
            HotKeyManager.RegisterHotKey(shortcut.Key, shortcut.Modifier);
        }

        public void AddActionEventListener(IActionEventListener actionEventListener)
        {
            eventListeners.Add(actionEventListener);
        }

        public void Dispose()
        {
            HotKeyManager.HotKeyPressed -= eventHandler;
            eventListeners.Clear();
        }
    }
}