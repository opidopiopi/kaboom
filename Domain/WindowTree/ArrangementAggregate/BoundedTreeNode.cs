using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.General;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public abstract class BoundedTreeNode : IBoundedTreeNode
    {
        protected List<IBoundedTreeNode> Children = new List<IBoundedTreeNode>();

        public EntityID ID { get; } = new EntityID();
        public Bounds Bounds { get; set; }

        public void InsertAsFirst(IBoundedTreeNode child) => Children.Insert(0, child);
        public void InsertAsLast(IBoundedTreeNode child) => Children.Add(child);
        public void Remove(IBoundedTreeNode node) => Children.Remove(node);
        public bool IsLeaf() => false;


        public void InsertAfter(IBoundedTreeNode node, IBoundedTreeNode reference)
        {
            if (!Children.Contains(reference))
            {
                throw new Exception($"The given reference: {reference}, is not a child of this node: {this}");
            }
            else
            {
                Children.Insert(Children.IndexOf(reference) + 1, node);
            }
        }

        public void InsertBefore(IBoundedTreeNode node, IBoundedTreeNode reference)
        {
            if (!Children.Contains(reference))
            {
                throw new Exception($"The given reference: {reference}, is not a child of this node: {this}");
            }
            else
            {
                Children.Insert(Children.IndexOf(reference), node);
            }
        }

        public abstract void Accept(IVisitor visitor);
        public void VisitAllChildren(IVisitor visitor) => Children.ForEach(child => child.Accept(visitor));
    }
}