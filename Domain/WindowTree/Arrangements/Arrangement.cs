using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Exceptions;
using System.Collections.Generic;

namespace Kaboom.Domain.WindowTree.Arrangements
{
    public abstract class Arrangement : ITreeNode, IAcceptVisitors
    {
        protected ITreeNodeRepository m_treeNodeRepo;

        protected Arrangement(ITreeNodeRepository treeNodeRepo)
        {
            m_treeNodeRepo = treeNodeRepo;
        }

        public List<TreeNodeID> Children { get; } = new List<TreeNodeID>();
        public TreeNodeID ID { get; } = new TreeNodeID();


        public abstract bool SupportsAxis(Axis axis);

        public abstract bool CanIMoveChild(Axis axis, Direction direction, TreeNodeID child);

        public abstract void MoveChild(Axis axis, Direction direction, TreeNodeID child);


        public void InsertAsFirst(TreeNodeID child)
        {
            Children.Insert(0, child);
        }

        public void InsertAsLast(TreeNodeID child)
        {
            Children.Add(child);
        }

        public bool IsLeaf()
        {
            return false;
        }

        public void Remove(TreeNodeID child)
        {
            Children.Remove(child);
        }

        public TreeNodeID FirstChild()
        {
            return Children.Count > 0 ? Children[0] : null;
        }

        public TreeNodeID LastChild()
        {
            return Children.Count > 0 ? Children[Children.Count - 1] : null;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected void AssertSupportsAxisAndNodeIsDirectChild(Axis axis, Direction direction, TreeNodeID node)
        {
            if (!SupportsAxis(axis))
            {
                throw new UnsupportedAxis($"This arrangement (this: {this}) does not support moving a child in axis: {axis}");
            }

            if (!Children.Contains(node))
            {
                throw new GivenNodeIsNotADirectChild($"The given node {node} is not a direct child of this arrangement. (this: {this})");
            }
        }
    }
}
