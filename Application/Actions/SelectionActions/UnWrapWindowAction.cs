using Kaboom.Domain;

namespace Kaboom.Application.Actions.SelectionActions
{
    public class UnWrapWindowAction : SelectionAction
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