using Kaboom.Abstract;
using Kaboom.Model;

namespace Testing.Mocks
{
    public class MockWindowProvider : IProvideWindows
    {
        private IAcceptWindows m_windowAcceptor;

        public void SetWindowAcceptor(IAcceptWindows windowAcceptor)
        {
            m_windowAcceptor = windowAcceptor;
        }

        public void InsertWindow(IWindowIdentity windowIdentity)
        {
            m_windowAcceptor.InsertWindow(windowIdentity, new Rectangle(0, 0, 1, 1));
        }

        public void InsertWindow(IWindowIdentity windowIdentity, Rectangle windowBounds)
        {
            m_windowAcceptor.InsertWindow(windowIdentity, windowBounds);
        }

        public void RemoveWindow(IWindowIdentity windowIdentity)
        {
            m_windowAcceptor.RemoveWindow(windowIdentity);
        }
    }
}
