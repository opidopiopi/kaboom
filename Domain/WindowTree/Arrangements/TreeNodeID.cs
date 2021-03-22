using System;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public class TreeNodeID
    {
        private Guid m_guid = Guid.NewGuid();

        public override bool Equals(object obj)
        {
            return obj is TreeNodeID iD &&
                   m_guid.Equals(iD.m_guid);
        }

        public override int GetHashCode()
        {
            return 628244108 + m_guid.GetHashCode();
        }

        public override string ToString()
        {
            return m_guid.ToString();
        }
    }
}
