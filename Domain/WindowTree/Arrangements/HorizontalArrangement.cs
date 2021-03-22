namespace Kaboom.Domain.WindowTree.Arrangements
{
    public class HorizontalArrangement : Arrangement
    {
        public HorizontalArrangement(ITreeNodeRepository treeNodeRepo) : base(treeNodeRepo)
        {

        }

        public override bool CanIMoveChild(Axis axis, Direction direction, TreeNodeID child)
        {
            if (!SupportsAxis(axis) || !Children.Contains(child))
            {
                return false;
            }

            if (Children.IndexOf(child) == 0)
            {
                return direction == Direction.NEGATIVE;
            }
            else if(Children.IndexOf(child) == Children.Count - 1)
            {
                return direction == Direction.POSITIVE;
            }

            return true;
        }

        public override void MoveChild(Axis axis, Direction direction, TreeNodeID child)
        {
            AssertSupportsAxisAndNodeIsDirectChild(axis, direction, child);

            int index = Children.IndexOf(child);

            int indexToBe = index + ((direction == Direction.POSITIVE) ? -1 : 1);

            ITreeNode nodeAtIndexToBe = m_treeNodeRepo.Find(Children[indexToBe]);
            if (nodeAtIndexToBe.IsLeaf())
            {
                Children.RemoveAt(index);
                Children.Insert(indexToBe, child);
            }
            else
            {
                if(direction == Direction.POSITIVE)
                {
                    nodeAtIndexToBe.InsertAsLast(child);
                }
                else
                {
                    nodeAtIndexToBe.InsertAsFirst(child);
                }
                Remove(child);
            }
        }

        public override bool SupportsAxis(Axis axis)
        {
            return axis == Axis.X;
        }
    }
}
