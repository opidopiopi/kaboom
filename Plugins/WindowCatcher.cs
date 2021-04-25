using Kaboom.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    public class WindowCatcher : IDisposable
    {
        private IWorkspace m_workspace;
        private WindowMapper m_windowMapper;
        private List<IntPtr> m_windows = new List<IntPtr>();
        private List<IntPtr> m_eventHooks = new List<IntPtr>();
        private Win32Wrapper.WinEventDelegate m_eventDelegate;

        private long m_lastUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public WindowCatcher(WindowMapper windowMapper, IWorkspace workspace)
        {
            m_windowMapper = windowMapper;
            m_workspace = workspace;

            m_eventDelegate = new Win32Wrapper.WinEventDelegate(WindowEventsCallback);
            HookEvents();
        }

        public void RunUpdateLoop()
        {
            Win32Wrapper.MSG msg;
            while (Win32Wrapper.GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
            {
                Win32Wrapper.TranslateMessage(ref msg);
                Win32Wrapper.DispatchMessage(ref msg);
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
                info.dwStyle != 0 &&
                Win32Wrapper.IsWindowVisible(windowHandle) &&
                string.IsNullOrEmpty(name) == false &&
                !name.Equals(WindowsWindowRenderer.OVERLAY_NAME) &&
                (info.dwStyle & (uint)Win32Wrapper.WindowStyles.WS_POPUP) == 0 &&
                Win32Wrapper.IsWindow(windowHandle) == true
            );
        }

        private void PrepareWindow(IntPtr windowHandle)
        {
            Win32Wrapper.ShowWindow(windowHandle, /*SW_RESTORE*/ 9);

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

        public void WindowEventsCallback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if(eventType == Win32Wrapper.EVENT_OBJECT_CREATE || eventType == Win32Wrapper.EVENT_OBJECT_DESTROY)
            {
                if(DateTimeOffset.Now.ToUnixTimeMilliseconds() - m_lastUpdate > 10)
                {
                    UpdateWindowsAndTriggerEvents();
                    m_lastUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
            }
            else if (eventType == Win32Wrapper.EVENT_SYSTEM_FOREGROUND && m_windows.Contains(hwnd))
            {
                var window = m_windowMapper.MapToDomain(hwnd);

                if (window != null && !window.ID.Equals(m_workspace.SelectedWindow))
                {
                    m_workspace.SelectWindow(window.ID);
                }
            }
        }

        private void HookEvents()
        {
            m_eventHooks.Add(Win32Wrapper.SetWinEventHook(
                Win32Wrapper.EVENT_SYSTEM_FOREGROUND,
                Win32Wrapper.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero,
                m_eventDelegate,
                0, 0,
                Win32Wrapper.WINEVENT_OUTOFCONTEXT | Win32Wrapper.WINEVENT_SKIPOWNPROCESS
                ));

            m_eventHooks.Add(Win32Wrapper.SetWinEventHook(
                Win32Wrapper.EVENT_OBJECT_CREATE,
                Win32Wrapper.EVENT_OBJECT_DESTROY,
                IntPtr.Zero,
                m_eventDelegate,
                0, 0,
                Win32Wrapper.WINEVENT_OUTOFCONTEXT | Win32Wrapper.WINEVENT_SKIPOWNPROCESS
                ));
        }

        private void UnHookEvents()
        {
            m_eventHooks.ForEach(hook => Win32Wrapper.UnhookWinEvent(hook));
            m_eventHooks.Clear();
        }

        public void Dispose()
        {
            UnHookEvents();
        }
    }
}
