using Kaboom.Adapters;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Plugins.Overlay
{
    class IconRenderer : IVisitor
    {
        private const int ICON_SIZE = 32;
        private const int LINE_HEIGHT = ICON_SIZE + 4;

        private int xPosition;
        private int yPosition;
        private Graphics graphics;
        private WindowMapper windowMapper;

        public IconRenderer(int xPosition, int yPosition, Graphics graphics, WindowMapper windowMapper)
        {
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            this.graphics = graphics;
            this.windowMapper = windowMapper;
        }

        public void Visit(Arrangement arrangement)
        {
            string text;

            if(arrangement is HorizontalArrangement)
            {
                text = "H";
            }
            else if(arrangement is VerticalArrangement)
            {
                text = "V";
            }
            else if (arrangement is StackArrangement)
            {
                text = "S";
            }
            else
            {
                throw new Exception($"Unsupported arrangement type: {arrangement.GetType()}");
            }

            DrawString(text, arrangement.Visible);
        }

        public void Visit(Window window)
        {
            DrawIcon(window, window.Visible);
        }

        private void DrawString(string text, bool isSelected)
        {
            Font drawFont = new Font("Arial", 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            DrawBackGround(isSelected);

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.DrawString(text, drawFont, drawBrush, xPosition, yPosition);

            yPosition += LINE_HEIGHT;
        }

        private void DrawIcon(Window window, bool isSelected)
        {
            var icon = GetIconForWindow(windowMapper.MapToIWindow(window)).ToBitmap();
            icon.MakeTransparent(Color.Transparent);

            DrawBackGround(isSelected);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.DrawImage(
                icon,
                xPosition,
                yPosition,
                ICON_SIZE,
                ICON_SIZE
            );

            yPosition += LINE_HEIGHT;
        }

        private void DrawBackGround(bool isSelected)
        {
            graphics.FillRectangle(Brushes.WhiteSmoke, xPosition - 1, yPosition - 1, ICON_SIZE + 2, ICON_SIZE + 2);
            if(!isSelected) graphics.DrawRectangle(new Pen(Color.White, 4), xPosition - 1, yPosition - 1, ICON_SIZE + 2, ICON_SIZE + 2);
        }

        private Icon GetIconForWindow(IWindow window)
        {
            uint processID;
            Win32Wrapper.GetWindowThreadProcessId(window.WindowHandle, out processID);

            return GetIconForProcess((int)processID);
        }

        private Icon GetIconForProcess(int processID)
        {
            return Icon.ExtractAssociatedIcon(Process.GetProcessById(processID).MainModule.FileName);
        }
    }
}