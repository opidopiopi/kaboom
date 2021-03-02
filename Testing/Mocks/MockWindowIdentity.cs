using Kaboom.Model;
using System;

namespace Testing.Mocks
{
    public class MockWindowIdentity : IWindowIdentity
    {
        private int m_identity;

        public MockWindowIdentity(int identity)
        {
            m_identity = identity;
        }

        public override bool Equals(object obj)
        {
            return obj is MockWindowIdentity identity &&
                   m_identity == identity.m_identity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(m_identity);
        }

        public override string ToString()
        {
            return $"MockWindowIdentity({m_identity})";
        }
    }
}
