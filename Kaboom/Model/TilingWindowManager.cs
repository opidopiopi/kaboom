using Kaboom.Abstract;
using System.Collections.Generic;

namespace Kaboom.Model
{
    public class TilingWindowManager
    {
        private Workspace m_workspace;
        private IProvideWindows m_windowProvider;
        private ISetWindowBounds m_windowBoundsSetter;
        private Dictionary<IWindowIdentity, Window> m_windows = new Dictionary<IWindowIdentity, Window>();

        public TilingWindowManager(IProvideWindows windowProvider, IProvideScreens screenProvider, ISetWindowBounds windowBoundsSetter)
        {
            m_workspace = new Workspace(screenProvider);

            m_windowProvider = windowProvider;
            m_windowProvider.SetNewWindowCallback(InsertNewWindow);
            m_windowProvider.SetRemoveWindowCallback(RemoveWindow);

            m_windowBoundsSetter = windowBoundsSetter;
        }

        private void InsertNewWindow(IWindowIdentity identity, Rectangle bounds)
        {
            if (m_windows.ContainsKey(identity))
            {
                return;
            }

            Window newWindow = new Window(identity, bounds, m_windowBoundsSetter);
            m_windows[identity] = newWindow;
            m_workspace.Insert(newWindow);
        }

        private void RemoveWindow(IWindowIdentity identity)
        {
            if (!m_windows.ContainsKey(identity)){
                return;
            }

            m_workspace.RemoveAndReturnSuccess(m_windows[identity]);
        }

        public Workspace GetWorkspace()
        {
            return m_workspace;
        }
    }
}
