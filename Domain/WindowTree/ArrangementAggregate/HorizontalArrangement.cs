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
            if (!SupportsAxis(direction.Axis) || FindChild(childID) == null)
            {
                return null;
            }

            var child = FindChild(childID);
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
            throw new System.NotImplementedException();
        }
    }
}
