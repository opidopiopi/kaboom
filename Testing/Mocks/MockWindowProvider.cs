using Kaboom.Model;

namespace Testing.Mocks
{
    public class MockWindowProvider : IProvideWindows
    {
        private NewWindowCallback m_newWindowCallback;
        private RemoveWindowCallback m_removeWindowCallback;

        public void SetNewWindowCallback(NewWindowCallback callback)
        {
            m_newWindowCallback = callback;
        }

        public void SetRemoveWindowCallback(RemoveWindowCallback callback)
        {
            m_removeWindowCallback = callback;
        }

        public void InsertWindow(IWindowIdentity windowIdentity)
        {
            m_newWindowCallback(windowIdentity, new Kaboom.Abstract.Rectangle(0, 0, 1, 1));
        }

        public void InsertWindow(IWindowIdentity windowIdentity, Kaboom.Abstract.Rectangle windowBounds)
        {
            m_newWindowCallback(windowIdentity, windowBounds);
        }

        public void RemoveWindow(IWindowIdentity windowIdentity)
        {
            m_removeWindowCallback(windowIdentity);
        }
    }
}
