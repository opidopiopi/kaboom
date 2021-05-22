namespace Kaboom.Domain.WindowTree.Helpers
{
    public class ParentFinder : IVisitor
    {
        private Arrangement root;
        private EntityID childID;

        private Arrangement parent;

        public ParentFinder(Arrangement root, EntityID targetID)
        {
            this.root = root;
            childID = targetID;
        }

        public Arrangement FindParentInTree()
        {
            Visit(root);

            return parent;
        }

        public void Visit(Arrangement arrangement)
        {
            if (arrangement.FindChild(childID) != null)
            {
                parent = arrangement;
            }
            else if (parent == null)
            {
                arrangement.VisitAllChildren(this);
            }
        }

        public void Visit(Window window)
        {

        }
    }
}