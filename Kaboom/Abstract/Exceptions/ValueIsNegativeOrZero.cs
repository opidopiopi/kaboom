using System;

namespace Kaboom.Abstract.Exceptions
{
    public class ValueIsNegativeOrZero : Exception
    {
        public ValueIsNegativeOrZero()
        {
        }

        public ValueIsNegativeOrZero(string message)
            : base(message)
        {
        }

        public ValueIsNegativeOrZero(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
