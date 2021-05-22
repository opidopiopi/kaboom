using Kaboom.Domain;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Plugins
{
    public class WindowsArrangementReposititory<DefaultArrangementType> : IArrangementRepository
        where DefaultArrangementType : Arrangement, new()
    {
        private List<Arrangement> m_rootArrangements = new List<Arrangement>();

        public WindowsArrangementReposititory()
        {
            m_rootArrangements = Screen.AllScreens.Select(
                screen => new DefaultArrangementType
                {
                    Bounds = new Bounds(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height)
                }
            ).ToList<Arrangement>();
        }

        public Arrangement AnyRoot()
        {
            return m_rootArrangements.FirstOrDefault();
        }

        public Arrangement Find(EntityID arrangementID)
        {
            return m_rootArrangements.Where(arrrangement => arrrangement.ID.Equals(arrangementID)).FirstOrDefault();
        }

        public Arrangement FindNeighbourOfRoot(EntityID rootID, Direction direction)
        {
            var root = m_rootArrangements.Find(arr => arr.ID.Equals(rootID));
            if (root == null)
            {
                throw new Exception($"Repository does not contain a root with ID: {rootID}!!");
            }

            var candidates = m_rootArrangements.Where(candidate => !candidate.Equals(root));
            candidates = DirectionFilterForArrangements.FilterForDirection(direction, root, candidates);

            return CandidateThatIsTheClosestToTheRoot(root, candidates);
        }

        public Arrangement FindParentOf(EntityID arrangementOrWindow)
        {
            return m_rootArrangements
                .Select(root => root.FindParentOf(arrangementOrWindow))
                .Where(parent => parent != null)
                .FirstOrDefault();
        }

        public Arrangement FindThatContainsPoint(int x, int y)
        {
            return m_rootArrangements
                .Where(arr => arr.Bounds.IsPointInside(x, y))
                .FirstOrDefault();
        }

        public void InsertRoot(Arrangement arrangement)
        {
            m_rootArrangements.Add(arrangement);
        }

        public void RemoveRoot(EntityID arrangementID)
        {
            m_rootArrangements.RemoveAll(arr => arr.ID.Equals(arrangementID));
        }

        public List<EntityID> RootArrangements()
        {
            return m_rootArrangements.Select(arr => arr.ID).ToList();
        }

        private static Arrangement CandidateThatIsTheClosestToTheRoot(Arrangement root, IEnumerable<Arrangement> candidates)
        {
            return candidates.OrderBy(arrangement =>
            {
                double deltaX = arrangement.Bounds.X - root.Bounds.X;
                double deltaY = arrangement.Bounds.Y - root.Bounds.Y;

                return deltaX * deltaX + deltaY * deltaY;
            }).FirstOrDefault();
        }
    }
}