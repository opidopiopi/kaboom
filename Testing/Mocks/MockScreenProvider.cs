using Kaboom.Abstract;
using Kaboom.Model;
using System.Collections.Generic;

namespace Testing.Mocks
{
    public class MockScreenProvider : IProvideScreens
    {
        private List<Rectangle> m_screens;

        public MockScreenProvider(List<Rectangle> screens)
        {
            m_screens = screens;
        }

        public List<Rectangle> GetScreenBounds()
        {
            return m_screens;
        }
    }
}
