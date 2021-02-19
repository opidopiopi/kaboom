using Kaboom.Model;
using System.Collections.Generic;
using Testing.Mocks;
using Kaboom.Abstract;

namespace Testing
{
    public class IntegrationTestBase
    {
        protected Screen m_screenA;
        protected Screen m_screenB;

        protected MockScreenProvider m_screenProvider;
        protected MockWindowProvider m_windowProvider;
        protected MockWindowBoundsSetter m_windowBoundsSetter;

        protected TilingWindowManager m_windowManager;

        public void Initialize()
        {
            m_screenA = new Screen(new Rectangle(0, 0, 1920, 1080));
            m_screenB = new Screen(new Rectangle(0, 1080, 1920, 1080));

            m_screenProvider = new MockScreenProvider(new List<Rectangle>() { m_screenA.Bounds, m_screenB.Bounds });
            m_windowProvider = new MockWindowProvider();
            m_windowBoundsSetter = new MockWindowBoundsSetter();

            m_windowManager = new TilingWindowManager(m_windowProvider, m_screenProvider, m_windowBoundsSetter);

            m_screenA = (Screen)m_windowManager.GetWorkspace().Children()[0];
            m_screenB = (Screen)m_windowManager.GetWorkspace().Children()[1];
        }
    }
}
