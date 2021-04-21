using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Plugins
{
    public class FormNotShowingInAltTab : Form
    {
        private const int SELECTED_WINDOW_BORDER_WIDTH = 5;

        private IntPtr m_selectedWindowHandle = IntPtr.Zero;
        private IntPtr m_eventHook = IntPtr.Zero;
        private readonly Win32Wrapper.WinEventDelegate m_updateCallback;

        public FormNotShowingInAltTab()
        {
            Text = WindowsWindowRenderer.OVERLAY_NAME;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            TransparencyKey = Color.White;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;

            Bounds = Screen.AllScreens.Select(screen => screen.Bounds).Aggregate((a, b) =>
                {
                    int minX = a.X < b.X ? a.X : b.X;
                    int minY = a.Y < b.Y ? a.Y : b.Y;
                    int maxX = a.X + a.Width > b.X + b.Width ? a.X + a.Width : b.X + b.Width;
                    int maxY = a.Y + a.Height > b.Y + b.Height ? a.Y + a.Height : b.Y + b.Height;

                    return new Rectangle(
                        minX,
                        minY,
                        maxX - minX,
                        maxY - minY
                    );
                });
            Paint += OverlayUpdate;

            m_updateCallback = new Win32Wrapper.WinEventDelegate(SelectedWindowMovedEvent);
            Load += (sender, args) => HookEvent();
            FormClosing += (sender, args) => UnHookEvent();
        }

        private void OverlayUpdate(object sender, PaintEventArgs e)
        {
            if (m_selectedWindowHandle != IntPtr.Zero && Win32Wrapper.IsWindow(m_selectedWindowHandle))
            {
                Win32Wrapper.RECT windowRect;
                Win32Wrapper.DwmGetWindowAttribute(m_selectedWindowHandle, Win32Wrapper.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out windowRect, sizeof(int) * 4);

                var form = sender as FormNotShowingInAltTab;
                windowRect.X -= form.Bounds.X;
                windowRect.Y -= form.Bounds.Y;

                var graphics = e.Graphics;
                var pen = new Pen(Color.WhiteSmoke)
                {
                    Width = SELECTED_WINDOW_BORDER_WIDTH,
                    Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
                };

                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                graphics.DrawRectangle(
                    pen,
                    windowRect.X - WindowsWindowRenderer.WINDOW_BORDER_WIDTH,
                    windowRect.Y - WindowsWindowRenderer.WINDOW_BORDER_WIDTH,
                    windowRect.Width + 2 * WindowsWindowRenderer.WINDOW_BORDER_WIDTH,
                    windowRect.Height + 2 * WindowsWindowRenderer.WINDOW_BORDER_WIDTH
                );
            }
            else
            {
                m_selectedWindowHandle = IntPtr.Zero;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= (int) Win32Wrapper.WindowStyles.WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        public void SetSelectedWindowHandle(IntPtr windowHandle)
        {
            m_selectedWindowHandle = windowHandle;
            Invoke(new MethodInvoker(delegate { Refresh(); }));
        }

        private void HookEvent()
        {
            m_eventHook = Win32Wrapper.SetWinEventHook(
                Win32Wrapper.EVENT_OBJECT_LOCATIONCHANGE,
                Win32Wrapper.EVENT_OBJECT_LOCATIONCHANGE,
                IntPtr.Zero,
                m_updateCallback,
                0, 0,
                Win32Wrapper.WINEVENT_OUTOFCONTEXT
                );
        }

        private void UnHookEvent()
        {
            Win32Wrapper.UnhookWinEvent(m_eventHook);
            m_eventHook = IntPtr.Zero;
        }

        private void SelectedWindowMovedEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (hwnd == m_selectedWindowHandle)
            {
                Refresh();
            }
        }
    }
}