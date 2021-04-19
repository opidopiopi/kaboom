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
            var keyChar = key.ToString();

            if(keyChar.Length > 1)
            {
                throw new System.Exception($"Illegal key: '{key}' !!");
            }

            switch (modifier)
            {
                case KeyModifiers.Alt:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.ALT, keyChar[0]);
                case KeyModifiers.Control:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.CTRL, keyChar[0]);
                case KeyModifiers.Shift:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.SHIFT, keyChar[0]);
                case KeyModifiers.Windows:
                    return new Kaboom.Domain.ShortcutActions.Shortcut(Modifier.WINDOWS, keyChar[0]);
                default:
                    throw new System.Exception($"Illegal Modifier: {modifier} !");
            }
        }
    }
}
