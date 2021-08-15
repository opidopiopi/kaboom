using Kaboom.Adapters;
using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree;
using Plugins.Overlay;
using System.Collections.Generic;
using System.Linq;

namespace Plugins
{
    public class WindowsRenderService : IRenderService
    {
        public const int WINDOW_BORDER_SIZE = 5;
        public const string OVERLAY_NAME = "Kaboom_overlay";

        private WindowMapper windowMapper;
        private IOverlay overlay;
        private SelectionHighlight selectionHighlight = new SelectionHighlight();
        private List<StackArrangementVisualizer> stackArrangementVisualizers = new List<StackArrangementVisualizer>();

        public WindowsRenderService(WindowMapper mapper, IOverlay overlay)
        {
            this.windowMapper = mapper;
            this.overlay = overlay;

            overlay.AddComponent(selectionHighlight);
        }

        public void RenderTrees(IEnumerable<Arrangement> rootArrangements)
        {
            stackArrangementVisualizers.ForEach(visualizer => overlay.RemoveComponent(visualizer));
            stackArrangementVisualizers.Clear();

            rootArrangements.ToList().ForEach(root => root.Accept(this));
            stackArrangementVisualizers.ForEach(visualizer => overlay.AddComponent(visualizer));

            overlay.ReRender();
        }

        public void HighlightWindow(Window selectedWindow)
        {
            var iWindow = windowMapper.MapToIWindow(selectedWindow);
            iWindow.PutInForground();
            
            selectionHighlight.SetSelectedWindow(iWindow);
            overlay.ReRender();

            Render(selectedWindow);
        }

        public void Visit(Arrangement arrangement)
        {
            if(arrangement is StackArrangement stackArrangement)
            {
                stackArrangementVisualizers.Add(new StackArrangementVisualizer(stackArrangement, windowMapper));
            }

            arrangement.VisitAllChildren(this);
        }

        public void Visit(Window window)
        {
            if (window.Visible)
            {
                Render(window);
            }
            else
            {
                windowMapper.MapToIWindow(window).MoveToBack();
            }
        }

        private void Render(Window window)
        {
            windowMapper
                .MapToIWindow(window)
                .ApplyRect(
                    WINDOW_BORDER_SIZE,
                    RectangleMapper.BoundsToRectangle(window.Bounds)
                );
        }
    }
}