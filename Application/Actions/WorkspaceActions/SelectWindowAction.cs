using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application.Actions.WorkspaceActions
{
    public class SelectWindowAction : WorkspaceAction
    {
        private Direction m_direction;

        public SelectWindowAction(ISelection selection, Direction direction) : base(selection)
        {
            m_direction = direction;
        }

        public override void Execute()
        {
            m_selection.MoveSelection(m_direction);
        }
    }
}
