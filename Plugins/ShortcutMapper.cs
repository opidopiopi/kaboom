using Kaboom.Domain.ShortcutActions;
using System.Windows.Forms;

namespace Plugins
{
    public class ShortcutMapper
    {
        public static Keys GetKeyFromShortcut(Kaboom.Domain.ShortcutActions.Shortcut shortcut)
        {
            //see: https://stackoverflow.com/a/10518085
            return (Keys)char.ToUpper(shortcut.Key);
        }

        public static KeyModifiers GetModifierFromShortcut(Kaboom.Domain.ShortcutActions.Shortcut shortcut)
        {
            switch (shortcut.Modifier)
            {
                case Modifier.WINDOWS:
                    return KeyModifiers.Windows;
                case Modifier.SHIFT:
                    return KeyModifiers.Shift;
                case Modifier.CTRL:
                    return KeyModifiers.Control;
                case Modifier.ALT:
                    return KeyModifiers.Alt;
                default:
                    throw new System.Exception($"No mapping for Modifier: {shortcut.Modifier} exists!");
            }
        }

        public static Kaboom.Domain.ShortcutActions.Shortcut MapToShortcut(Keys key, KeyModifiers modifier)
        {
            var keyChar = (char) key;

            switch (modifier)
            {
                case KeyModifiers.Alt:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.ALT, keyChar);
                case KeyModifiers.Control:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.CTRL, keyChar);
                case KeyModifiers.Shift:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.SHIFT, keyChar);
                case KeyModifiers.Windows:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.WINDOWS, keyChar);
                default:
                    throw new System.Exception($"Illegal Modifier: {modifier} !");
            }
        }
    }
}
