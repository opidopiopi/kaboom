namespace Kaboom.Application.Actions.WorkspaceActions
{
    public abstract class WorkspaceAction : IAction
    {
        protected ISelection m_selection;

        protected WorkspaceAction(ISelection selection)
        {
            m_selection = selection;
        }

        public abstract void Execute();
    }
}
