﻿using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Testing.Mocks
{
    public class MockTreeLeaf : BoundedTreeLeaf
    {
        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
