using System;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    public class CannotInsertChild : Exception
    {
        public CannotInsertChild(string message)
            : base(message)
        {
        }
    }
}
