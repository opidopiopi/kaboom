using Kaboom.Domain;
using Kaboom.Domain.WindowTree;

namespace Kaboom.Application.Actions.SelectionActions
{
    public class WrapWindowAction<ArrangementType> : SelectionAction
        where ArrangementType : Arrangement, new()
    {
        public WrapWindowAction(ISelection selection) : base(selection)
        {
        }

        public override void Execute()
        {
            m_selection.WrapSelectedWindow(new ArrangementType());
        }
    }
}
