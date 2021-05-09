using Kaboom.Domain.WindowTree.Exceptions;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Domain.WindowTree.Helpers
{
    public abstract class BoundedTreeLeaf : IBoundedTreeNode
    {
        public EntityID ID { get; } = new EntityID();
        public Bounds Bounds { get; set; }


        public void InsertAsFirst(IBoundedTreeNode child) => throw new CannotInsertChild("Cannot insert children into Leaf node!");
        public void InsertAsLast(IBoundedTreeNode child) => throw new CannotInsertChild("Cannot insert children into Leaf node!");

        public void InsertBefore(IBoundedTreeNode node, IBoundedTreeNode reference) => throw new CannotInsertChild("Cannot insert children into Leaf node!");
        public void InsertAfter(IBoundedTreeNode node, IBoundedTreeNode reference) => throw new CannotInsertChild("Cannot insert children into Leaf node!");

        public void Remove(IBoundedTreeNode node) => throw new CannotRemoveChild("Leaf nodes have no children to remove!");

        public bool IsLeaf() => true;

        public abstract void Accept(IVisitor visitor);
    }
}