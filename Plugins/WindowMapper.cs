using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plugins
{
    public class WindowMapper
    {
        private Dictionary<IntPtr, Window> m_windowInfo = new Dictionary<IntPtr, Window>();

        public Window MapToDomain(IntPtr windowHandle)
        {
            if (m_windowInfo.ContainsKey(windowHandle))
            {
                return m_windowInfo[windowHandle];
            }
            else
            {
                StringBuilder name = new StringBuilder(255);
                Win32Wrapper.RECT rect;
                Win32Wrapper.DwmGetWindowAttribute(windowHandle, Win32Wrapper.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out rect, sizeof(int) * 4);
                Win32Wrapper.GetWindowTextW(windowHandle, name, name.Capacity + 1);

                Window window = new Window(new Kaboom.Abstraction.Rectangle(rect.X, rect.Y, rect.Width, rect.Height), name.ToString());
                m_windowInfo[windowHandle] = window;

                return window;
            }
        }

        public IntPtr MapToWindowHandle(EntityID windowID)
        {
            return m_windowInfo.FirstOrDefault(info => info.Value.ID.Equals(windowID)).Key;
        }

        internal void RemoveMappingForHandle(IntPtr handle)
        {
            m_windowInfo.Remove(handle);
        }

        internal bool HasMappingFor(IntPtr handle)
        {
            return m_windowInfo.ContainsKey(handle);
        }
    }
}
