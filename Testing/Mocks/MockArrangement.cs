using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Mocks
{
    public class MockArrangement : Arrangement
    {
        public bool Updated = false;
        public string Title = "";

        public MockArrangement(params Axis[] axes)
            : base(axes)
        {
        }

        public MockArrangement(string title, params Axis[] axes)
            : base(axes)
        {
            Title = title;
        }

        public override EntityID NeighbourOfChildInDirection(EntityID childID, Direction direction)
        {
            int index = Children.IndexOf(FindChild(childID));

            index += (direction == Direction.Up || direction == Direction.Left) ? -1 : 1;

            if(index < 0 || index >= Children.Count)
            {
                return null;
            }
            else
            {
                return Children[index].ID;
            }
        }

        public override void UpdateBoundsOfChildren()
        {
            Updated = true;
            Children.ForEach(child => {
                if(child is Arrangement arrangement)
                {
                    arrangement.UpdateBoundsOfChildren();
                }
            });
        }

        public List<IBoundedTreeNode> MyChildren => Children;

        public override string ToString()
        {
            return $"(MockArrangement Title: {Title}, ID: {ID})";
        }
    }
}
