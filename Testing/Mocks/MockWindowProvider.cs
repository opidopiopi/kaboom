using Kaboom.Model;

namespace Testing
{
    public class MockWindowProvider : IProvideWindows
    {
        private WindowCallback m_newWindowCallback;
        private WindowCallback m_removeWindowCallback;

        public void SetNewWindowCallback(WindowCallback callback)
        {
            m_newWindowCallback = callback;
        }

        public void SetRemoveWindowCallback(WindowCallback callback)
        {
            m_removeWindowCallback = callback;
        }

        public void InsertWindow(Window window)
        {
            m_newWindowCallback(window);
        }

        public void RemoveWindow(Window window)
        {
            m_removeWindowCallback(window);
        }
    }
}
