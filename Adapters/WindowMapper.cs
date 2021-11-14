using Kaboom.Domain.WindowTree;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Adapters
{
    public class WindowMapper
    {
        private Dictionary<IWindow, Window> m_windowInfo = new Dictionary<IWindow, Window>();

        public Window MapToDomain(IWindow iWindow)
        {
            if (m_windowInfo.ContainsKey(iWindow))
            {
                return m_windowInfo[iWindow];
            }
            else
            {
                Window window = new Window(
                    RectangleMapper.RectangleToBounds(iWindow.GetActualWindowRect()),
                    iWindow.WindowName()
                );
                m_windowInfo[iWindow] = window;

                return window;
            }
        }

        public IWindow MapToIWindow(Window window)
        {
            var iWindow = m_windowInfo.FirstOrDefault(info => info.Value.Equals(window)).Key;

            return iWindow;
        }

        public void RemoveMappingForWindow(IWindow window)
        {
            m_windowInfo.Remove(window);
        }
    }
}