using Kaboom.Application;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using System.Threading;
using System.Windows.Forms;

namespace Plugins
{
    public class WindowsWindowRenderer : IWindowRenderer
    {
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

            Win32Wrapper.MoveWindow(
                handle,
                window.Bounds.X + withBorder.X - noBorder.X,
                window.Bounds.Y + withBorder.Y - noBorder.Y,
                window.Bounds.Width + withBorder.Width - noBorder.Width,
                window.Bounds.Height + withBorder.Height - noBorder.Height,
                true
            );
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