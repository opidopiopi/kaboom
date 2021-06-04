using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Domain.WindowTree
{
    public class HorizontalArrangement : Arrangement
    {
        public HorizontalArrangement()
            : base(new Axis[] { Axis.X })
        {
        }

        public override EntityID NeighbourOfChildInDirection(EntityID childID, Direction direction)
        {
            var child = FindChild(childID);

            if (!SupportsAxis(direction.Axis) || child == null)
            {
                return null;
            }

            int index = Children.IndexOf(child);
            int neighbourIndex = index + (direction.Equals(Direction.Left) ? -1 : 1);

            if (neighbourIndex >= 0 && neighbourIndex <= Children.Count - 1)
            {
                return Children[neighbourIndex].ID;
            }

            return null;
        }

        public override void UpdateBoundsOfChildren()
        {
            if (Children.Count == 0) return;

            int numChildren = Children.Count;
            int widthPerChild = Bounds.Width / numChildren;

            for (int i = 0; i < numChildren; i++)
            {
                Children[i].Bounds = new Bounds(Bounds.X + i * widthPerChild, Bounds.Y, widthPerChild, Bounds.Height);
            }
        }
    }
}
