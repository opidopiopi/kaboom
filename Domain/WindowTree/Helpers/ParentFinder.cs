namespace Kaboom.Domain.WindowTree.Helpers
{
    public class ParentFinder : IVisitor
    {
        private Arrangement root;
        private EntityID childID;

        private Arrangement parent;

        private ParentFinder(Arrangement root, EntityID targetID)
        {
            this.root = root;
            childID = targetID;
        }

        private Arrangement Find()
        {
            Visit(root);

            return parent;
        }

        public static Arrangement FindParentInTree(Arrangement root, EntityID targetID)
        {
            return new ParentFinder(root, targetID).Find();
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