using Kaboom.Adapters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Plugins
{
    [ExcludeFromCodeCoverage]
    public class FormNotShowingInAltTab : Form
    {
        private const int SELECTED_WINDOW_BORDER_WIDTH = 5;

        private IWindow m_selectedWindow = null;
        private IntPtr m_eventHook = IntPtr.Zero;
        private readonly Win32Wrapper.WinEventDelegate m_updateCallback;

        public FormNotShowingInAltTab()
        {
            Text = WindowsRenderService.OVERLAY_NAME;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            TransparencyKey = Color.White;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;

            //let the form span over all screens
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
            if (m_selectedWindow != null && Win32Wrapper.IsWindow(m_selectedWindow.WindowHandle))
            {
                var windowRect = m_selectedWindow.GetActualWindowRect();

                //the hidden Form spans over all screens so there might be an offset
                //between the coordinates of a window and the coordinates in the graphics
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
                    windowRect.X - WindowsRenderService.WINDOW_BORDER_Size,
                    windowRect.Y - WindowsRenderService.WINDOW_BORDER_Size,
                    windowRect.Width + 2 * WindowsRenderService.WINDOW_BORDER_Size,
                    windowRect.Height + 2 * WindowsRenderService.WINDOW_BORDER_Size
                );
            }
        }

        //make this form not show up in ALt+Tab
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

        public void SetSelectedWindow(IWindow window)
        {
            m_selectedWindow = window;
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
            if (m_selectedWindow != null && hwnd == m_selectedWindow.WindowHandle)
            {
                Refresh();
            }
        }
    }
}