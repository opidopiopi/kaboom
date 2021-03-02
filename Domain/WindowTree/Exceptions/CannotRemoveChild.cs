using System;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    public class CannotRemoveChild : Exception
    {
        public CannotRemoveChild(string message)
            : base(message)
        {
        }
    }
}
