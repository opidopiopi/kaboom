namespace Kaboom.Model
{
    public class TilingWindowManager
    {
        private Workspace m_workspace;
        private IProvideWindows m_windowProvider;

        public TilingWindowManager(IProvideWindows windowProvider, IProvideScreens screenProvider, ISetWindowBounds windowBoundsSetter)
        {
            m_workspace = new Workspace(screenProvider, windowBoundsSetter);

            m_windowProvider = windowProvider;
            m_windowProvider.SetWindowAcceptor(m_workspace);    //in the future we can use a proxy if we want multiple workspaces
        }

        public Workspace GetWorkspace()
        {
            return m_workspace;
        }
    }
}
