﻿using System;

namespace Kaboom.Domain
{
    public class EntityID
    {
        private Guid ID = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            return obj is EntityID iD &&
                   ID.Equals(iD.ID);
        }

        public override int GetHashCode()
        {
            return 1213502048 + ID.GetHashCode();
        }

        public override string ToString()
        {
            return $"(EntityID: {ID})";
        }
    }
}
