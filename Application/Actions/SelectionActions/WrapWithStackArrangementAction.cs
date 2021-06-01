using Kaboom.Domain;
using Kaboom.Domain.WindowTree;

namespace Kaboom.Application.Actions.SelectionActions
{
    public class WrapWithStackArrangementAction : SelectionAction
    {
        public WrapWithStackArrangementAction(ISelection selection) : base(selection)
        {
        }

        public override void Execute()
        {
            m_selection.WrapSelectedWindow(new StackArrangement(m_selection));
        }
    }
}