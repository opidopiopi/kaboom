using System;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    public class GivenNodeIsNotADirectChild : Exception
    {
        public GivenNodeIsNotADirectChild(string message)
            : base(message)
        {
        }
    }
}
