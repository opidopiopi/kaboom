using Kaboom.Domain.ShortcutActions;

namespace Kaboom.Application.WorkspaceActions
{
    public class UnWrapWindowAction : WorkspaceAction
    {
        public UnWrapWindowAction(Shortcut shortcut, Workspace workspace) : base(shortcut, workspace)
        {
        }

        public override void Execute(IActionTarget actionTarget)
        {
            m_workspace.UnWrapSelectedWindow();
        }
    }
}
