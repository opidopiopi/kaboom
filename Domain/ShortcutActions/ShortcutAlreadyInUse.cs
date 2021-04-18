using System;

namespace Kaboom.Domain.ShortcutActions
{
    public class ShortcutAlreadyInUse : Exception
    {
        public ShortcutAlreadyInUse(string message)
            : base(message)
        {
        }
    }
}
