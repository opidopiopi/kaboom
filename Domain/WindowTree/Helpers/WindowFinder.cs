namespace Kaboom.Domain.WindowTree.Helpers
{
    public class WindowFinder : IVisitor
    {
        private const bool FORWARD = false;
        private const bool REVERSED = true;

        private EntityID windowID;
        private readonly bool reversed;

        private WindowFinder(bool reversed)
        {
            this.windowID = null;
            this.reversed = reversed;
        }

        private EntityID FindWindow(Arrangement root)
        {
            Visit(root);

            return windowID;
        }

        public static EntityID FirstWindowUnderArrangement(Arrangement arrangement)
        {
            return new WindowFinder(FORWARD).FindWindow(arrangement);
        }

        public static EntityID LastWindowUnderArrangement(Arrangement arrangement)
        {
            return new WindowFinder(REVERSED).FindWindow(arrangement);
        }

        public void Visit(Arrangement arrangement)
        {
            if (windowID == null)
            {
                if (reversed)
                {
                    arrangement.VisitAllChildrenReverse(this);
                }
                else
                {
                    arrangement.VisitAllChildren(this);
                }
            }
        }

        public void Visit(Window window)
        {
            if(windowID == null)
            {
                windowID = window.ID;
            }
        }
    }
}