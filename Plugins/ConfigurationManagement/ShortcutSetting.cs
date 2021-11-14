using Kaboom.Application.Actions;
using Kaboom.Application.ConfigurationManagement;
using Plugins.Shortcuts;
using System;

namespace Plugins.ConfigurationManagement
{
    public class ShortcutSetting : Setting
    {
        private IListenToShortcuts m_shortcutListener;
        private IActionEventListener m_eventListener;
        private IAction m_action;

        public ShortcutSetting(string name, string defaultValue, IListenToShortcuts shortcutListener, IActionEventListener eventListener, IAction action)
            : base(name, defaultValue)
        {
            m_shortcutListener = shortcutListener;
            m_eventListener = eventListener;
            m_action = action;
        }

        protected override void Apply(string value)
        {
            var shortcut = ParseShortcut(value);
            m_shortcutListener.RegisterShortcut(shortcut);
            m_eventListener.RegisterActionForEvent(shortcut, m_action);
        }

        private static Shortcut ParseShortcut(string shortcut)
        {
            var splitted = shortcut.Split(' ');

            if (splitted.Length != 2)
            {
                throw new ArgumentException($"Invalid shortcut defined: '{shortcut}'");
            }

            try
            {
                KeyModifiers modifier = (KeyModifiers)Enum.Parse(typeof(KeyModifiers), splitted[0]);
                System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), splitted[1]);

                return new Shortcut(modifier, key);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid shortcut defined: '{shortcut}'");
            }
        }
    }
}