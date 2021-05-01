namespace Kaboom.Application.Actions.WorkspaceActions
{
    public class UnWrapWindowAction : WorkspaceAction
    {
        public UnWrapWindowAction(ISelection selection) : base(selection)
        {
        }

        public override void Execute()
        {
            m_selection.UnWrapSelectedWindow();
        }
    }
}