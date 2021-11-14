using Kaboom.Domain.WindowTree.ValueObjects;
using System.Drawing;

namespace Kaboom.Adapters
{
    public static class RectangleMapper
    {
        public static Bounds RectangleToBounds(Rectangle rectangle)
        {
            return new Bounds(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle BoundsToRectangle(Bounds bounds)
        {
            return new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }
    }
}