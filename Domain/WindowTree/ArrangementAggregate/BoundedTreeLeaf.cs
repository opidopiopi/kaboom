﻿using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.Exceptions;
using System;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public abstract class BoundedTreeLeaf : IBoundedTreeNode
    {
        public EntityID ID { get; } = new EntityID();
        public Rectangle Bounds { get; set; }


        public void InsertAsFirst(IBoundedTreeNode child) => throw new CannotInsertChild("Cannot insert children into Leaf node!");
        public void InsertAsLast(IBoundedTreeNode child) => throw new CannotInsertChild("Cannot insert children into Leaf node!");

        public void InsertBefore(IBoundedTreeNode node, IBoundedTreeNode reference) => throw new CannotInsertChild("Cannot insert children into Leaf node!");
        public void InsertAfter(IBoundedTreeNode node, IBoundedTreeNode reference) => throw new CannotInsertChild("Cannot insert children into Leaf node!");

        public void Remove(IBoundedTreeNode node) => throw new CannotRemoveChild("Leaf nodes have no children to remove!");

        public IBoundedTreeNode FirstChild() => throw new Exception("Leaf nodes have no children!");
        public IBoundedTreeNode LastChild() => throw new Exception("Leaf nodes have no children!");

        public bool IsLeaf() => true;
    }
}