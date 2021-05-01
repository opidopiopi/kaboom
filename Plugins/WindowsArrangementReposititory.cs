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
            m_rootArrangements = Screen.AllScreens.Select(screen =>
            {
                return new DefaultArrangementType
                {
                    Bounds = new Bounds(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height)
                };
            }).ToList<Arrangement>();
        }

        public Arrangement AnyRoot()
        {
            return (m_rootArrangements.Count > 0) ? m_rootArrangements[0] : null;
        }

        public Arrangement Find(EntityID arrangementID)
        {
            return m_rootArrangements.Where(arr => arr.ID.Equals(arrangementID)).FirstOrDefault();
        }

        public Arrangement FindNeighbourOfRootInDirection(EntityID arrangementID, Direction direction)
        {
            var root = m_rootArrangements.Find(arr => arr.ID.Equals(arrangementID));
            if(root == null)
            {
                throw new System.Exception($"Repository does not contain a root with ID: {arrangementID}!!");
            }

            var candidates = m_rootArrangements.Where(arr => !arr.Equals(root));
            if(direction.Axis == Axis.X)
            {
                candidates = candidates.Where(arrangement =>
                {
                    return arrangement.Bounds.Y + arrangement.Bounds.Height >= root.Bounds.Y && arrangement.Bounds.Y <= root.Bounds.Y + root.Bounds.Height;
                });

                if (direction == Direction.Left)
                {
                    candidates = candidates.Where(arrangement => arrangement.Bounds.X < root.Bounds.X);
                }
                else
                {
                    candidates = candidates.Where(arrangement => arrangement.Bounds.X > root.Bounds.X);
                }
            }
            else
            {
                candidates = candidates.Where(arrangement =>
                {
                    return arrangement.Bounds.X + arrangement.Bounds.Width >= root.Bounds.X && arrangement.Bounds.X <= root.Bounds.X + root.Bounds.Width;
                });

                if (direction == Direction.Up)
                {
                    candidates = candidates.Where(arrangement => arrangement.Bounds.Y < root.Bounds.Y);
                }
                else
                {
                    candidates = candidates.Where(arrangement => arrangement.Bounds.Y > root.Bounds.Y);
                }
            }

            if (candidates.Count() == 0)
            {
                return null;
            }

            return candidates.OrderBy(arr =>
            {
                double dx = arr.Bounds.X - root.Bounds.X;
                double dy = arr.Bounds.Y - root.Bounds.Y;

                return Math.Sqrt(dx * dx + dy * dy);
            }).First();
        }

        public Arrangement FindParentOf(EntityID arrangementOrWindow)
        {
            foreach (var child in m_rootArrangements)
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
            return m_rootArrangements.Where(arr => arr.Bounds.IsPointInside(x, y)).FirstOrDefault();
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
    }
}
