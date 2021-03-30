using Kaboom.Abstraction;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public class HorizontalArrangement : Arrangement
    {
        public override void MoveChild(ITreeNode child, Direction direction)
        {
            AssertSupportsAxisAndNodeIsDirectChild(direction.Axis, child);

            int index = Children.IndexOf(child);
            int indexToBe = index + (direction.Equals(Direction.Left) ? -1 : 1);

            if (Children[indexToBe].IsLeaf())
            {
                Children.RemoveAt(index);
                Children.Insert(indexToBe, child);
            }
            else
            {
                if (direction == Direction.Left)
                {
                    Children[indexToBe].InsertAsLast(child);
                }
                else if(direction == Direction.Right)
                {
                    Children[indexToBe].InsertAsFirst(child);
                }
                Remove(child);
            }
        }

        public override ITreeNode NeighbourOfChildInDirection(ITreeNode treeNode, Direction direction)
        {
            if (!SupportsAxis(direction.Axis) || !Children.Contains(treeNode))
            {
                return null;
            }

            int index = Children.IndexOf(treeNode);
            int neighbourIndex = index + (direction.Equals(Direction.Left) ? -1 : 1);

            if(neighbourIndex >= 0 && neighbourIndex <= Children.Count - 1)
            {
                return Children[neighbourIndex];
            }

            return null;
        }

        public override bool SupportsAxis(Axis axis)
        {
            return axis == Axis.X;
        }
    }
}
