using System;

namespace Kaboom.Domain.WindowTree.Window
{
    public class WindowIdentity
    {
        private Guid m_guid = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            return obj is WindowIdentity identity &&
                   m_guid.Equals(identity.m_guid);
        }

        public override int GetHashCode()
        {
            return 628244108 + m_guid.GetHashCode();
        }
    }
}
