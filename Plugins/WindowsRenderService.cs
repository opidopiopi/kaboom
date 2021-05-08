using Kaboom.Adapters;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using System.Threading;
using System.Windows.Forms;

namespace Plugins
{
    public class WindowsRenderService : IRenderService
    {
        public const int WINDOW_BORDER_Size = 5;
        public const string OVERLAY_NAME = "Kaboom_overlay";

        private WindowMapper m_mapper;
        private FormNotShowingInAltTab m_graphicsForm;

        public WindowsRenderService(WindowMapper mapper)
        {
            m_mapper = mapper;

            PrepareForm();
        }

        public void ExecuteFromRoot(Arrangement rootArrangement)
        {
            rootArrangement.Accept(this);
        }

        public void HighlightWindow(Window selectedWindow)
        {
            var iWindow = m_mapper.MapToIWindow(selectedWindow);
            m_graphicsForm.SetSelectedWindow(iWindow);
            iWindow.PutInForground();

            Render(selectedWindow);
        }

        public void Visit(Arrangement arrangement)
        {
            arrangement.VisitAllChildren(this);
        }

        public void Visit(Window window)
        {
            Render(window);
        }

        private void Render(Window window)
        {
            m_mapper.MapToIWindow(window).ApplyPreferredRect(WINDOW_BORDER_Size);
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