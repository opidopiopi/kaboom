using Kaboom.Application.Actions;
using System.Windows.Forms;

namespace Plugins.Shortcuts
{
    public class Shortcut : IActionEvent
    {
        public readonly KeyModifiers Modifier;
        public readonly Keys Key;

        public Shortcut(KeyModifiers modifier, Keys key)
        {
            Modifier = modifier;
            Key = key;
        }

        public bool Equals(IActionEvent actionEvent)
        {
            return Equals(actionEvent as object);
        }

        public override bool Equals(object obj)
        {
            return obj is Shortcut shortcut &&
                   Modifier == shortcut.Modifier &&
                   Key == shortcut.Key;
        }

        public override int GetHashCode()
        {
            int hashCode = 503722236;
            hashCode = hashCode * -1521134295 + Modifier.GetHashCode();
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            return hashCode;
        }
    }
}