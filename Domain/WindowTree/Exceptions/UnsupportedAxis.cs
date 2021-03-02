using System;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    public class UnsupportedAxis : Exception
    {
        public UnsupportedAxis(string message)
            : base(message)
        {
        }
    }
}
