using Kaboom.Adapters;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using Plugins.Overlay;

namespace Plugins
{
    public class WindowsRenderService : IRenderService
    {
        public const int WINDOW_BORDER_Size = 5;
        public const string OVERLAY_NAME = "Kaboom_overlay";

        private WindowMapper m_mapper;
        private IOverlay overlay;
        private SelectionHighlight selectionHighlight = new SelectionHighlight();

        public WindowsRenderService(WindowMapper mapper, IOverlay overlay)
        {
            m_mapper = mapper;
            this.overlay = overlay;

            overlay.AddComponent(selectionHighlight);
        }

        public void ExecuteFromRoot(Arrangement rootArrangement)
        {
            rootArrangement.Accept(this);
            overlay.ReRender();
        }

        public void HighlightWindow(Window selectedWindow)
        {
            var iWindow = m_mapper.MapToIWindow(selectedWindow);
            iWindow.PutInForground();
            
            selectionHighlight.SetSelectedWindow(iWindow);
            overlay.ReRender();

            Render(selectedWindow);
        }

        public void Visit(Arrangement arrangement)
        {
            arrangement.VisitAllChildren(this);
        }

        public void Visit(Window window)
        {
            if (window.Visible)
            {
                Render(window);
            }
        }

        private void Render(Window window)
        {
            m_mapper
                .MapToIWindow(window)
                .ApplyRect(
                    WINDOW_BORDER_Size,
                    RectangleMapper.BoundsToRectangle(window.Bounds)
                );
        }
    }
}