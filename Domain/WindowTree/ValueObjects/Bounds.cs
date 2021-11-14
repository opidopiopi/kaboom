using Kaboom.Abstraction.Exceptions;

namespace Kaboom.Domain.WindowTree.ValueObjects
{
    public class Bounds
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Bounds(int x, int y, int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ValueIsNegativeOrZero(
                    string.Format(
                        "One or both of the following values is invalid: width: {0} height: {1}. Only non zero positive values are allowed!",
                        width,
                        height
                    ));
            }

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool IsPointInside(int x, int y)
        {
            return X <= x && x < X + Width &&
                    Y <= y && y < Y + Height;
        }

        public override string ToString()
        {
            return $"Bounds(X: {X}, Y: {Y}, Width: {Width}, Height: {Height})";
        }

        public override bool Equals(object obj)
        {
            return obj is Bounds rectangle &&
                   X == rectangle.X &&
                   Y == rectangle.Y &&
                   Width == rectangle.Width &&
                   Height == rectangle.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}
