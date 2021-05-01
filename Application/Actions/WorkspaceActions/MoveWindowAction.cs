using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application.Actions.WorkspaceActions
{
    public class MoveWindowAction : WorkspaceAction
    {
        private Direction m_direction;

        public MoveWindowAction(ISelection selection, Direction direction) : base(selection)
        {
            m_direction = direction;
        }

        public override void Execute()
        {
            m_selection.MoveSelectedWindow(m_direction);
        }
    }
}
