using Kaboom.Adapters;
using Kaboom.Domain.WindowTree;
using System.Drawing;

namespace Plugins.Overlay
{
    public class StackArrangementVisualizer : IOverlayComponent
    {
        private const int X_OFFSET = 5;
        private const int Y_OFFSET = 5;

        private StackArrangement arrangement;
        private WindowMapper windowMapper;

        public StackArrangementVisualizer(StackArrangement arrangement, WindowMapper windowMapper)
        {
            this.arrangement = arrangement;
            this.windowMapper = windowMapper;
        }

        public void Render(Graphics graphics)
        {
            arrangement.VisitAllChildren(
                new IconRenderer(
                    arrangement.Bounds.X + X_OFFSET,
                    arrangement.Bounds.Y + Y_OFFSET,
                    graphics,
                    windowMapper
                )
            );
        }
    }
}
