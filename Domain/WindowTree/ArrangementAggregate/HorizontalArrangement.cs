using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
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
            int numChildren = Children.Count;
            int widthPerChild = Bounds.Width / numChildren;

            for(int i = 0; i < numChildren; i++)
            {
                Children[i].Bounds = new Rectangle(Bounds.X + i * widthPerChild, Bounds.Y, widthPerChild, Bounds.Height);

                if(Children[i] is Arrangement arrangement)
                {
                    arrangement.UpdateBoundsOfChildren();
                }
            }
        }
    }
}
