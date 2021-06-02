using System.Drawing;

namespace Plugins.Overlay
{
    public interface IOverlayComponent
    {
        void Render(Graphics graphics);
    }
}