using Kaboom.Adapters;
using Kaboom.Domain;
using Kaboom.Domain.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    public class WindowCatcher : IDisposable
    {
        private ISelection m_selection;
        private IWindowService m_windowService;
        private WindowMapper m_windowMapper;
        private IWindowCatchingRule m_catchingRule;
        private List<WindowsWindow> m_windows = new List<WindowsWindow>();
        private List<IntPtr> m_eventHooks = new List<IntPtr>();
        private Win32Wrapper.WinEventDelegate m_eventDelegate;

        private long m_lastUpdate = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public WindowCatcher(WindowMapper windowMapper, ISelection selection, IWindowService windowService, IWindowCatchingRule catchingRule)
        {
            m_windowMapper = windowMapper;
            m_selection = selection;
            m_catchingRule = catchingRule;

            m_eventDelegate = new Win32Wrapper.WinEventDelegate(WindowEventsCallback);
            HookEvents();
            m_windowService = windowService;
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

            newWindows.ForEach(window =>
            {
                window.PrepareForInsertion();
                m_windowService.InsertWindowIntoTree(m_windowMapper.MapToDomain(window), m_selection);
                Console.WriteLine($"[WindowCatcher]         New Window: {window.WindowHandle}");
            });

            removeWindows.ForEach(window =>
            {
                m_windowService.RemoveWindowFromTree(m_windowMapper.MapToDomain(window).ID, m_selection);
                m_windowMapper.RemoveMappingForWindow(window);
                Console.WriteLine($"[WindowCatcher]         Removed Window: {window.WindowName()}");
            });
            
            m_windows = currentWindows;
        }

        private List<WindowsWindow> FetchAllWindows()
        {
            List<WindowsWindow> handles = new List<WindowsWindow>();

            Win32Wrapper.EnumDesktopWindows(
                IntPtr.Zero,
                new Win32Wrapper.EnumDesktopWindowsDelegate((handle, paramhandle) =>
            {
                var window = new WindowsWindow(handle);
                if (m_catchingRule.DoWeWantToCatchThisWindow(window))
                {
                    handles.Add(window);
                }

                return true;
            }), IntPtr.Zero);

            return handles;
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
            else if (eventType == Win32Wrapper.EVENT_SYSTEM_FOREGROUND && m_windows.Exists(window => window.WindowHandle.Equals(hwnd)))
            {
                var window = m_windowMapper.MapToDomain(new WindowsWindow(hwnd));

                if (window != null && !window.ID.Equals(m_selection.SelectedWindow))
                {
                    m_selection.SelectWindow(window.ID);
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