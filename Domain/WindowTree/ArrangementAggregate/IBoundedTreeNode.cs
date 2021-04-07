﻿using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Domain.WindowTree.ArrangementAggregate
{
    public interface IBoundedTreeNode : ITreeNode<IBoundedTreeNode>, IHaveBounds, IEntity
    {

    }
}
