using Kaboom.Adapters;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Plugins
{
    public class WindowsWindow : IWindow
    {
        public IntPtr WindowHandle { get; }

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

        public void ApplyRect(int borderSize, Rectangle rectangle)
        {
            var rectangleToSet = AdjustToBorder(borderSize, rectangle);

            Win32Wrapper.SetWindowPos(
                WindowHandle,
                Win32Wrapper.HWND_TOPMOST,
                rectangleToSet.X,
                rectangleToSet.Y,
                rectangleToSet.Width,
                rectangleToSet.Height,
                (uint) Win32Wrapper.SetWindowPosFlags.ShowWindow
            );
        }

        public void MoveToBack()
        {
            Win32Wrapper.SetWindowPos(
                WindowHandle,
                Win32Wrapper.HWND_NOTOPMOST,
                0, 0, 0, 0,
                (uint) (Win32Wrapper.SetWindowPosFlags.IgnoreMove | Win32Wrapper.SetWindowPosFlags.IgnoreResize)
            );
        }

        public string WindowName()
        {
            return Win32Wrapper.GetWindowName(WindowHandle);
        }

        /// <summary>
        /// Un-maximize the window and call setWindowOnce to apply
        /// </summary>
        public void PrepareForInsertion()
        {
            Restore();

            var rect = GetActualWindowRect();
            Win32Wrapper.SetWindowPos(
                WindowHandle,
                Win32Wrapper.HWND_TOP,
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height,
                (uint) Win32Wrapper.SetWindowPosFlags.ShowWindow);
        }

        public void Minimize()
        {
            Win32Wrapper.ShowWindow(WindowHandle, (int) Win32Wrapper.ShowWindowFlags.SW_MINIMIZE);
        }

        public void Restore()
        {
            Win32Wrapper.ShowWindow(WindowHandle, (int)Win32Wrapper.ShowWindowFlags.SW_RESTORE);
        }

        public void PutInForground()
        {
            if (Win32Wrapper.GetForegroundWindow() != WindowHandle)
            {
                Win32Wrapper.SetForegroundWindow(WindowHandle);
            }
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

        private Rectangle AdjustToBorder(int borderSize, Rectangle rectangle)
        {
            //windows might have an invisible border that we want to get rid of
            Win32Wrapper.RECT withBorder;
            Win32Wrapper.GetWindowRect(WindowHandle, out withBorder);
            var noBorder = GetActualWindowRect();

            int xOffset = withBorder.X - noBorder.X;
            int yOffset = withBorder.Y - noBorder.Y;
            int widthOffset = withBorder.Width - noBorder.Width;
            int heightOffset = withBorder.Height - noBorder.Height;

            return new Rectangle(
                rectangle.X + xOffset + borderSize,
                rectangle.Y + yOffset + borderSize,
                rectangle.Width + widthOffset - borderSize * 2,
                rectangle.Height + heightOffset - borderSize * 2
            );
        }
    }
}