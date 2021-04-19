using Kaboom.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Plugins
{
    public class WindowCatcher
    {
        private const int UPDATE_INTERVAL = 100;

        private IWorkspace m_workspace;
        private WindowMapper m_windowMapper;
        private List<IntPtr> m_windows = new List<IntPtr>();

        public WindowCatcher(WindowMapper windowMapper, IWorkspace workspace)
        {
            m_windowMapper = windowMapper;
            m_workspace = workspace;
        }

        public void RunUpdateLoop()
        {
            while (true)
            {
                UpdateWindowsAndTriggerEvents();
                Thread.Sleep(UPDATE_INTERVAL);
            }
        }

        private void UpdateWindowsAndTriggerEvents()
        {
            var currentWindows = FetchAllWindows();
            var newWindows = currentWindows.Except(m_windows).ToList();
            var removeWindows = m_windows.Except(currentWindows).ToList();

            newWindows.ForEach(handle =>
            {
                PrepareWindow(handle);
                m_workspace.InsertWindow(m_windowMapper.MapToDomain(handle));
                Console.WriteLine($"[WindowCatcher]         New Window: {Win32Wrapper.GetWindowName(handle)}");
            });

            removeWindows.ForEach(handle =>
            {
                m_workspace.RemoveWindow(m_windowMapper.MapToDomain(handle).ID);
                m_windowMapper.RemoveMappingForHandle(handle);
                Console.WriteLine($"[WindowCatcher]         Removed Window: {Win32Wrapper.GetWindowName(handle)}");
            });
            
            m_windows = currentWindows;
        }

        private List<IntPtr> FetchAllWindows()
        {
            List<IntPtr> handles = new List<IntPtr>();

            Win32Wrapper.EnumDesktopWindows(
                IntPtr.Zero,
                new Win32Wrapper.EnumDesktopWindowsDelegate((handle, paramhandle) =>
            {
                if (DoWeWantToCatchThisWindow(handle))
                {
                    handles.Add(handle);
                }

                return true;
            }), IntPtr.Zero);

            return handles;
        }

        private bool DoWeWantToCatchThisWindow(IntPtr windowHandle)
        {
            Win32Wrapper.WINDOWINFO info = new Win32Wrapper.WINDOWINFO(null);
            Win32Wrapper.GetWindowInfo(windowHandle, ref info);

            string name = Win32Wrapper.GetWindowName(windowHandle);

#if DEBUG
            if (name.Contains("Microsoft Visual Studio")) return false;
#endif

            return (
                Win32Wrapper.IsWindowVisible(windowHandle) &&
                string.IsNullOrEmpty(name) == false &&
                (info.dwStyle & (uint)Win32Wrapper.WindowStyles.WS_POPUP) == 0 &&
                Win32Wrapper.IsWindow(windowHandle) == true
            );
        }

        private void PrepareWindow(IntPtr windowHandle)
        {
            Win32Wrapper.ShowWindow(windowHandle, /*SW_RESTORE*/ 9);

            /*
            Win32Wrapper.SetWindowLongPtr(
                windowHandle,
                -0x10,  //Set window Style
                new IntPtr(
                    (long)(
                    Win32Wrapper.WindowStyles.WS_DLGFRAME |
                    Win32Wrapper.WindowStyles.WS_BORDER |
                    Win32Wrapper.WindowStyles.WS_SIZEBOX |
                    0
                    )));
            */

            var window = m_windowMapper.MapToDomain(windowHandle);
            Win32Wrapper.SetWindowPos(
                windowHandle,
                new IntPtr(0),
                window.Bounds.X,
                window.Bounds.Y,
                window.Bounds.Width,
                window.Bounds.Height,
                0x0040);
        }
    }
}
