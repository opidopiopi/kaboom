using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System.Threading;
using System.Windows.Forms;

namespace Plugins
{
    public class WindowsWindowRenderer : IRenderService
    {
        public const int WINDOW_BORDER_WIDTH = 5;
        public static string OVERLAY_NAME = "Kaboom_overlay";

        private WindowMapper m_mapper;
        private FormNotShowingInAltTab m_graphicsForm;

        public WindowsWindowRenderer(WindowMapper mapper)
        {
            m_mapper = mapper;

            PrepareForm();
        }

        public void HighlightWindow(Window selectedWindow)
        {
            var handle = m_mapper.MapToWindowHandle(selectedWindow.ID);
            m_graphicsForm.SetSelectedWindowHandle(handle);

            if (Win32Wrapper.GetForegroundWindow() != handle)
            {
                Win32Wrapper.SetForegroundWindow(handle);
            }

            Render(selectedWindow);
        }

        public void Render(Window window)
        {
            var handle = m_mapper.MapToWindowHandle(window.ID);

            //windows might have invisible border that we want to get rid of
            Win32Wrapper.RECT withBorder, noBorder;
            Win32Wrapper.GetWindowRect(handle, out withBorder);
            Win32Wrapper.DwmGetWindowAttribute(handle, Win32Wrapper.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out noBorder, sizeof(int) * 4);

            int xOffset = withBorder.X - noBorder.X;
            int yOffset = withBorder.Y - noBorder.Y;
            int widthOffset = withBorder.Width - noBorder.Width;
            int heightOffset = withBorder.Height - noBorder.Height;

            Win32Wrapper.MoveWindow(
                handle,
                window.Bounds.X + xOffset + WINDOW_BORDER_WIDTH,
                window.Bounds.Y + yOffset + WINDOW_BORDER_WIDTH,
                window.Bounds.Width + widthOffset - 2 * WINDOW_BORDER_WIDTH,
                window.Bounds.Height + heightOffset - 2 * WINDOW_BORDER_WIDTH,
                true
            );
        }

        public void Render(Arrangement arrangement)
        {
            throw new System.NotImplementedException();
        }

        private void PrepareForm()
        {
            m_graphicsForm = new FormNotShowingInAltTab();

            new Thread(() =>
            {
                Application.Run(m_graphicsForm);
            }).Start();
        }
    }
}