using Kaboom.Application.ConfigurationManagement;
using System;
using System.Windows.Forms;

namespace Plugins.ConfigurationManagement
{
    public delegate void ApplyShortcutSetting(Kaboom.Domain.ShortcutActions.Shortcut shortcut);

    public class ShortcutSetting : Setting
    {
        private ApplyShortcutSetting m_applyShortcut;

        public ShortcutSetting(string name, string defaultValue, ApplyShortcutSetting applyShortcut)
            : base(name, defaultValue)
        {
            m_applyShortcut = applyShortcut;
        }

        protected override void Apply(string value)
        {
            m_applyShortcut(ParseShortcut(value));
        }

        private static Kaboom.Domain.ShortcutActions.Shortcut ParseShortcut(string shortcut)
        {
            var splitted = shortcut.Split(' ');

            if (splitted.Length != 2)
            {
                throw new ArgumentException($"Invalid shortcut defined: '{shortcut}'");
            }

            try
            {
                KeyModifiers modifier = (KeyModifiers)Enum.Parse(typeof(KeyModifiers), splitted[0]);
                Keys key = (Keys)Enum.Parse(typeof(Keys), splitted[1]);

                return ShortcutMapper.MapToShortcut(key, modifier);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid shortcut defined: '{shortcut}'");
            }
        }
    }
}