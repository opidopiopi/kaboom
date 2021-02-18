using Kaboom.Model;

namespace Testing.Mocks
{
    public class MockWindowIdentity : IWindowIdentity
    {
        private int m_identity;

        public MockWindowIdentity(int identity)
        {
            m_identity = identity;
        }
    }
}
