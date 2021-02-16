using System;

namespace Kaboom.Model.Exceptions
{
    class NoScreensInWorkspace : Exception
    {
        public NoScreensInWorkspace()
        {
        }

        public NoScreensInWorkspace(string message)
            : base(message)
        {
        }

        public NoScreensInWorkspace(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
