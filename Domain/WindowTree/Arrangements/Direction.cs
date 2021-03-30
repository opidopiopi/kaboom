namespace Kaboom.Domain.WindowTree.Arrangements
{
    public class Direction
    {
        private enum DirectionType
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
        };

        public readonly Axis Axis;
        private readonly DirectionType m_directionType;

        private Direction(Axis axis, DirectionType directionType)
        {
            Axis = axis;
            m_directionType = directionType;
        }


        public static Direction Up => new Direction(Axis.Y, DirectionType.UP);
        public static Direction Down => new Direction(Axis.Y, DirectionType.DOWN);
        public static Direction Left => new Direction(Axis.X, DirectionType.LEFT);
        public static Direction Right => new Direction(Axis.X, DirectionType.RIGHT);

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   m_directionType == direction.m_directionType;
        }

        public override int GetHashCode()
        {
            return -300496486 + m_directionType.GetHashCode();
        }

        public static bool operator ==(Direction a, Direction b) => a.Equals(b);
        public static bool operator !=(Direction a, Direction b) => !a.Equals(b);
    }
}
