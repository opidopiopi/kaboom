using Kaboom.Model;
using System.Collections.Generic;

namespace Testing.Mocks
{
    public class MockScreenProvider : IProvideScreens
    {
        private List<Screen> m_screens;

        public MockScreenProvider(List<Screen> screens)
        {
            m_screens = screens;
        }

        public List<Screen> GetScreens()
        {
            return m_screens;
        }
    }
}
