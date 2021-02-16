using System;

namespace Kaboom.Model.Exceptions
{
    class ParentMustBeWorkspace : Exception
    {
        public ParentMustBeWorkspace()
        {
        }

        public ParentMustBeWorkspace(string message)
            : base(message)
        {
        }

        public ParentMustBeWorkspace(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
