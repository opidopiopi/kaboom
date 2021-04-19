using System.Collections.Generic;

namespace Kaboom.Domain.ShortcutActions
{
    public class Shortcut
    {
        public readonly Modifier Modifier;
        public readonly char Key;

        public Shortcut(Modifier modifier, char key)
        {
            Modifier = modifier;
            Key = key;
        }

        public override bool Equals(object obj)
        {
            return obj is Shortcut shortcut &&
                   EqualityComparer<Modifier>.Default.Equals(Modifier, shortcut.Modifier) &&
                   Key == shortcut.Key;
        }

        public override int GetHashCode()
        {
            int hashCode = 503722236;
            hashCode = hashCode * -1521134295 + EqualityComparer<Modifier>.Default.GetHashCode(Modifier);
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"(Shortcut: Mod: {Modifier}, Key: {Key})";
        }
    }
}
