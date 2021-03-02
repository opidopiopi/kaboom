using Kaboom.Abstraction;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public class HorizontalArrangement : Arrangement
    {
        public override bool CanIMoveChild(Axis axis, Direction direction, ITreeNode child)
        {
            if (!SupportsAxis(axis) || !m_children.Contains(child))
            {
                return false;
            }

            if (m_children.IndexOf(child) == 0)
            {
                return direction == Direction.NEGATIVE;
            }
            else if(m_children.IndexOf(child) == m_children.Count - 1)
            {
                return direction == Direction.POSITIVE;
            }

            return true;
        }

        public override void MoveChild(Axis axis, Direction direction, ITreeNode child)
        {
            AssertSupportsAxisAndNodeIsDirectChild(axis, direction, child);

            int index = m_children.IndexOf(child);

            int indexToBe = index + ((direction == Direction.POSITIVE) ? -1 : 1);

            if (m_children[indexToBe].IsLeaf())
            {
                m_children.RemoveAt(index);
                m_children.Insert(indexToBe, child);
            }
            else
            {
                if(direction == Direction.POSITIVE)
                {
                    m_children[indexToBe].InsertAsLast(child);
                }
                else
                {
                    m_children[indexToBe].InsertAsFirst(child);
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
