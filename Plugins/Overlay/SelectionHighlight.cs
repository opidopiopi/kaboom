﻿using Kaboom.Adapters;
using System.Drawing;

namespace Plugins.Overlay
{
    public class SelectionHighlight : IOverlayComponent
    {
        private const int SELECTED_WINDOW_BORDER_WIDTH = 5;

        private IWindow selectedWindow = null;
        private Pen pen = new Pen(Color.WhiteSmoke)
        {
            Width = SELECTED_WINDOW_BORDER_WIDTH,
            Alignment = System.Drawing.Drawing2D.PenAlignment.Inset,
        };

        public void Render(Graphics graphics)
        {
            if (selectedWindow != null && Win32Wrapper.IsWindow(selectedWindow.WindowHandle))
            {
                var windowRect = selectedWindow.GetActualWindowRect();

                graphics.DrawRectangle(
                    pen,
                    windowRect.X - WindowsRenderService.WINDOW_BORDER_SIZE,
                    windowRect.Y - WindowsRenderService.WINDOW_BORDER_SIZE,
                    windowRect.Width + 2 * WindowsRenderService.WINDOW_BORDER_SIZE,
                    windowRect.Height + 2 * WindowsRenderService.WINDOW_BORDER_SIZE
                );
            }
        }

        public void SetSelectedWindow(IWindow window)
        {
            selectedWindow = window;
        }
    }
}