using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;

namespace Kaboom.Application
{
    public class TreeUpdate : IVisitor
    {
        public void Visit(Arrangement arrangement)
        {
            arrangement.PropagateVisibility();

            if (arrangement.Visible)
            {
                arrangement.UpdateBoundsOfChildren();
            }

            arrangement.VisitAllChildren(this);
        }

        public void Visit(Window window)
        {

        }
    }
}