using System;
using System.Diagnostics.CodeAnalysis;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class GivenNodeIsNotADirectChild : Exception
    {
        public GivenNodeIsNotADirectChild(string message)
            : base(message)
        {
        }
    }
}
