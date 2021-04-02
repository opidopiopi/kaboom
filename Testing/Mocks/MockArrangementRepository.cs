using Kaboom.Abstraction;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Testing.Mocks
{
    public class MockArrangementRepository : IArrangementRepository
    {
        private List<Arrangement> m_arrangements = new List<Arrangement>();

        public Arrangement AnyRoot()
        {
            return m_arrangements.FirstOrDefault();
        }

        public Arrangement Find(EntityID arrangementID)
        {
            return m_arrangements.Where(arr => arr.ID.Equals(arrangementID)).FirstOrDefault();
        }

        public Arrangement FindParentOf(EntityID arrangementOrWindow)
        {
            foreach (var child in m_arrangements)
            {
                var res = child.FindParentOf(arrangementOrWindow);

                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

        public Arrangement FindThatContainsPoint(int x, int y)
        {
            return m_arrangements.Where(arr => arr.Bounds.IsPointInside(x, y)).FirstOrDefault();
        }

        public void InsertRoot(Arrangement arrangement)
        {
            m_arrangements.Add(arrangement);
        }

        public void RemoveRoot(EntityID arrangementID)
        {
            m_arrangements.RemoveAll(arr => arr.ID.Equals(arrangementID));
        }

        public List<EntityID> RootArrangements()
        {
            return m_arrangements.Select(arr => arr.ID).ToList();
        }
    }
}
