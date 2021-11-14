using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Plugins
{
    public static class DirectionFilterForArrangements
    {
        public static IEnumerable<Arrangement> FilterForDirection(Direction direction, Arrangement root, IEnumerable<Arrangement> candidates)
        {
            if (direction.Axis == Axis.X)
            {
                candidates = FilterForXAxis(direction, root, candidates);
            }
            else
            {
                candidates = FilterForYAxis(direction, root, candidates);
            }

            return candidates;
        }

        private static IEnumerable<Arrangement> FilterForXAxis(Direction direction, Arrangement root, IEnumerable<Arrangement> candidates)
        {
            candidates = OverlapsOnYAxisProjection(root, candidates);

            if (direction == Direction.Left)
            {
                return candidates.Where(arrangement => arrangement.Bounds.X < root.Bounds.X);
            }
            else
            {
                return candidates.Where(arrangement => arrangement.Bounds.X > root.Bounds.X);
            }
        }

        private static IEnumerable<Arrangement> FilterForYAxis(Direction direction, Arrangement root, IEnumerable<Arrangement> candidates)
        {
            candidates = OverlapsOnXAxisProjection(root, candidates);

            if (direction == Direction.Up)
            {
                return candidates.Where(arrangement => arrangement.Bounds.Y < root.Bounds.Y);
            }
            else
            {
                return candidates.Where(arrangement => arrangement.Bounds.Y > root.Bounds.Y);
            }
        }

        private static IEnumerable<Arrangement> OverlapsOnXAxisProjection(Arrangement root, IEnumerable<Arrangement> candidates)
        {
            return candidates.Where(arrangement =>
                arrangement.Bounds.X + arrangement.Bounds.Width >= root.Bounds.X && arrangement.Bounds.X <= root.Bounds.X + root.Bounds.Width
            );
        }

        private static IEnumerable<Arrangement> OverlapsOnYAxisProjection(Arrangement root, IEnumerable<Arrangement> candidates)
        {
            return candidates.Where(arrangement =>
                arrangement.Bounds.Y + arrangement.Bounds.Height >= root.Bounds.Y && arrangement.Bounds.Y <= root.Bounds.Y + root.Bounds.Height
            );
        }
    }
}