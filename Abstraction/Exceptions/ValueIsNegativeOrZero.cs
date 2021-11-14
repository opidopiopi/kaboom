using System;

namespace Kaboom.Abstraction.Exceptions
{
    public class ValueIsNegativeOrZero : Exception
    {
        public ValueIsNegativeOrZero(string message)
            : base(message)
        {
        }
    }
}
