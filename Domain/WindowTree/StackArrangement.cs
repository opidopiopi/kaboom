namespace Kaboom.Domain.WindowTree
{
    public class StackArrangement : VerticalArrangement
    {
        private EntityID lastSelected;
        private ISelection selection;
        private IArrangementRepository arrangementRepository;

        public StackArrangement(ISelection selection, IArrangementRepository arrangementRepository)
        {
            this.selection = selection;
            this.arrangementRepository = arrangementRepository;
        }

        public override void UpdateBoundsOfChildren()
        {
            if(FindChild(selection.SelectedWindow) != null)
            {
                lastSelected = selection.SelectedWindow;
            }
            else
            {
                var parentID = IHaveNoGoodNameForThisMethod();
                if (parentID != null)
                {
                    lastSelected = parentID;
                }
            }

            var topMostWindow = FindChild(lastSelected);
            if (topMostWindow == null)
            {
                topMostWindow = Children[0];
                lastSelected = topMostWindow.ID;
            }

            topMostWindow.Bounds = this.Bounds;
            foreach (var child in Children)
            {
                child.Visible = child.ID.Equals(lastSelected);
            }
        }

        private EntityID IHaveNoGoodNameForThisMethod()
        {
            EntityID parentID = selection.SelectedWindow;
            do
            {
                var parent = arrangementRepository.FindParentOf(parentID);

                if (parent == this)
                {
                    return parentID;
                }

                parentID = (parent == null)? null:parent.ID;
            } while (parentID != null);

            return null;
        }
    }
}