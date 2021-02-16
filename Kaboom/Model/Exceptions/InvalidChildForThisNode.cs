﻿using System;

namespace Kaboom.Model.Exceptions
{
    class InvalidChildForThisNode : Exception
    {
        public InvalidChildForThisNode()
        {
        }

        public InvalidChildForThisNode(string message)
            : base(message)
        {
        }

        public InvalidChildForThisNode(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
