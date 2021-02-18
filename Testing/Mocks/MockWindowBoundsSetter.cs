using Kaboom.Abstract;
using Kaboom.Model;
using System.Collections.Generic;

namespace Testing.Mocks
{
    public class MockWindowBoundsSetter : ISetWindowBounds
    {
        public Dictionary<IWindowIdentity, Rectangle> TheIdentitiesAndBoundsIHaveSet { get; } = new Dictionary<IWindowIdentity, Rectangle>();

        public void SetBoundsOfWindowWithIdentity(IWindowIdentity identity, Rectangle bounds)
        {
            TheIdentitiesAndBoundsIHaveSet[identity] = bounds;
        }
    }
}
