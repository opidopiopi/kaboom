using System;
using System.Diagnostics.CodeAnalysis;

namespace Kaboom.Domain.WindowTree.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UnsupportedAxis : Exception
    {
        public UnsupportedAxis(string message)
            : base(message)
        {
        }
    }
}
