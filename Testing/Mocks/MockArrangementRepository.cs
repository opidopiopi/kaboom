using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;
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

        public Arrangement FindNeighbourOfRoot(EntityID arrangementID, Direction direction)
        {
            int index = m_arrangements.IndexOf(m_arrangements.Find(arr => arr.ID.Equals(arrangementID)));
            index += direction == Direction.Up || direction == Direction.Left ? -1 : 1;

            if (index >= 0 && index < m_arrangements.Count)
            {
                return m_arrangements[index];
            }
            else
            {
                return null;
            }
        }

        public Arrangement FindParentOf(EntityID arrangementOrWindow)
        {
            foreach (var child in m_arrangements)
            {
                var res = ParentFinder.FindParentInTree(child, arrangementOrWindow);

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

        public List<Arrangement> RootArrangements()
        {
            return m_arrangements;
        }
    }
}
