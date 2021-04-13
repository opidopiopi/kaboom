using Kaboom.Domain.ShortcutActions;

namespace Kaboom.Application.WorkspaceActions
{
    public abstract class WorkspaceAction : Action
    {
        protected Workspace m_workspace;

        protected WorkspaceAction(Shortcut shortcut, Workspace workspace) : base(shortcut)
        {
            this.m_workspace = workspace;
        }
    }
}
