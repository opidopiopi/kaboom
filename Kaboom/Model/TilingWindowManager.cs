namespace Kaboom.Model
{
    public class TilingWindowManager
    {
        private Workspace m_workspace;
        private IProvideWindows m_windowProvider;

        public TilingWindowManager(IProvideWindows windowProvider, IProvideScreens screenProvider)
        {
            m_workspace = new Workspace(screenProvider);

            m_windowProvider = windowProvider;
            m_windowProvider.SetNewWindowCallback(InsertNewWindow);
            m_windowProvider.SetRemoveWindowCallback(RemoveWindow);
        }

        private void InsertNewWindow(Window window)
        {
            m_workspace.Insert(window);
        }

        private void RemoveWindow(Window window)
        {
            throw new System.NotImplementedException();
        }

        public Workspace GetWorkspace()
        {
            return m_workspace;
        }
    }
}
