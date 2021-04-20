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
            m_graphicsForm.SetSelectedWindowHandle(m_mapper.MapToWindowHandle(selectedWindow.ID));
            Render(selectedWindow);
        }

        public void Render(Window window)
        {
            Win32Wrapper.MoveWindow(
                m_mapper.MapToWindowHandle(window.ID),
                window.Bounds.X,
                window.Bounds.Y,
                window.Bounds.Width,
                window.Bounds.Height,
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