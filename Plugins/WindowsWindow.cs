using Kaboom.Adapters;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Plugins
{
    public class WindowsWindow : IWindow
    {
        public IntPtr WindowHandle { get; }
        public Rectangle PrefferedRectangle { get; set; }

        public WindowsWindow(IntPtr windowHandle)
        {
            WindowHandle = windowHandle;
        }

        public Rectangle GetActualWindowRect()
        {
            Win32Wrapper.RECT rect;
            Win32Wrapper.DwmGetWindowAttribute(WindowHandle, Win32Wrapper.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out rect, sizeof(int) * 4);

            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void ApplyPreferredRect(int borderSize)
        {
            //windows might have an invisible border that we want to get rid of
            Win32Wrapper.RECT withBorder;
            Win32Wrapper.GetWindowRect(WindowHandle, out withBorder);
            var noBorder = GetActualWindowRect();

            int xOffset = withBorder.X - noBorder.X;
            int yOffset = withBorder.Y - noBorder.Y;
            int widthOffset = withBorder.Width - noBorder.Width;
            int heightOffset = withBorder.Height - noBorder.Height;

            Win32Wrapper.MoveWindow(
                WindowHandle,
                PrefferedRectangle.X + xOffset + borderSize,
                PrefferedRectangle.Y + yOffset + borderSize,
                PrefferedRectangle.Width + widthOffset - 2 * borderSize,
                PrefferedRectangle.Height + heightOffset - 2 * borderSize,
                true
            );
        }

        public string WindowName()
        {
            return Win32Wrapper.GetWindowName(WindowHandle);
        }

        /// <summary>
        /// Un-maximize the window and call setWindowOnce to apply
        /// </summary>
        public void Prepare()
        {
            Win32Wrapper.ShowWindow(WindowHandle, /*SW_RESTORE*/ 9);

            var rect = GetActualWindowRect();
            Win32Wrapper.SetWindowPos(
                WindowHandle,
                new IntPtr(0),
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height,
                0x0040);
        }

        public override bool Equals(object obj)
        {
            return obj is WindowsWindow window &&
                   EqualityComparer<IntPtr>.Default.Equals(WindowHandle, window.WindowHandle);
        }

        public override int GetHashCode()
        {
            return 1407091763 + WindowHandle.GetHashCode();
        }

        public void PutInForground()
        {
            if (Win32Wrapper.GetForegroundWindow() != WindowHandle)
            {
                Win32Wrapper.SetForegroundWindow(WindowHandle);
            }
        }
    }
}