using Kaboom.Domain.ShortcutActions;

namespace Kaboom.Application.WorkspaceActions
{
    public class UnWrapWindowAction : WorkspaceAction
    {
        public UnWrapWindowAction(Shortcut shortcut, IWorkspace workspace) : base(shortcut, workspace)
        {
        }

        public override void Execute()
        {
            m_workspace.UnWrapSelectedWindow();
        }
    }
}
