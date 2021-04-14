using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application.WorkspaceActions
{
    public class MoveWindowAction : WorkspaceAction
    {
        private Direction m_direction;

        public MoveWindowAction(Shortcut shortcut, IWorkspace workspace, Direction direction) : base(shortcut, workspace)
        {
            m_direction = direction;
        }

        public override void Execute(IActionTarget actionTarget)
        {
            m_workspace.MoveSelectedWindow(m_direction);
        }
    }
}
