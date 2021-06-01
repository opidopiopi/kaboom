namespace Kaboom.Domain.WindowTree
{
    public class StackArrangement : VerticalArrangement
    {
        private EntityID m_lastSelected;
        private ISelection m_selection;

        public StackArrangement(ISelection selection)
        {
            m_selection = selection;
        }

        public override void UpdateBoundsOfChildren()
        {
            if (FindParentOf(m_selection.SelectedWindow) is Arrangement parent)
            {

            }

            var topMostWindow = FindChild(m_lastSelected);
            if(topMostWindow == null)
            {
                topMostWindow = Children[0];
                m_lastSelected = topMostWindow.ID;
            }

            topMostWindow.Bounds = this.Bounds;
            foreach (var child in Children)
            {
                child.Visible = child.ID.Equals(m_lastSelected);
            }
        }
    }
}