using Kaboom.Adapters;

namespace Plugins
{
    public class DefaultCatchingRule : IWindowCatchingRule
    {
        public bool DoWeWantToCatchThisWindow(IWindow window)
        {
            Win32Wrapper.WINDOWINFO info = new Win32Wrapper.WINDOWINFO(null);
            Win32Wrapper.GetWindowInfo(window.WindowHandle, ref info);

            string name = window.WindowName();

#if DEBUG
            if (name.Contains("Microsoft Visual Studio")) return false;
#endif

            return (
                info.dwStyle != 0 &&
                Win32Wrapper.IsWindowVisible(window.WindowHandle) &&
                string.IsNullOrEmpty(name) == false &&
                !name.Equals(WindowsRenderService.OVERLAY_NAME) &&
                (info.dwStyle & (uint)Win32Wrapper.WindowStyles.WS_POPUP) == 0 &&
                Win32Wrapper.IsWindow(window.WindowHandle) == true
            );
        }
    }
}
