﻿using Kaboom.Domain.ShortcutActions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    public class WindowsShortcutListener : IListenToShortcuts
    {
        public event EventHandler<ShortcutTriggeredEventArgs> ShortcutTriggered;

        public WindowsShortcutListener()
        {
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyPressed);
        }

        private void HotKeyPressed(object sender, HotKeyEventArgs args)
        {
            var eventArgs = new ShortcutTriggeredEventArgs()
            {
                Shortcut = ShortcutMapper.MapToShortcut(args.Key, args.Modifiers)
            };
            Console.WriteLine($"[ShortcutListener]     Shortcut pressed: {eventArgs.Shortcut}");

            ShortcutTriggered(this, eventArgs);
        }

        public void RegisterShortcut(Shortcut shortcut)
        {
            Console.WriteLine($"[ShortcutListener]     New Shortcut: {shortcut}");
            HotKeyManager.RegisterHotKey(ShortcutMapper.GetKeyFromShortcut(shortcut), ShortcutMapper.GetModifierFromShortcut(shortcut));
        }

        public void UnRegisterShortcut(Shortcut shortcut)
        {

        }
    }
}