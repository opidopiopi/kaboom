using Kaboom.Domain;

namespace Kaboom.Application.Actions.SelectionActions
{
    public abstract class SelectionAction : IAction
    {
        protected ISelection m_selection;

        protected SelectionAction(ISelection selection)
        {
            m_selection = selection;
        }

        public abstract void Execute();
    }
}
