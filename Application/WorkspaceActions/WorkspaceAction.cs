using Kaboom.Domain.ShortcutActions;

namespace Kaboom.Application.WorkspaceActions
{
    public abstract class WorkspaceAction : Action
    {
        protected IWorkspace m_workspace;

        protected WorkspaceAction(Shortcut shortcut, IWorkspace workspace) : base(shortcut)
        {
            this.m_workspace = workspace;
        }
    }
}
