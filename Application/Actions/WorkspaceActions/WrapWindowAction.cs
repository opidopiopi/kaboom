using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application.Actions.WorkspaceActions
{
    public class WrapWindowAction<ArrangementType> : WorkspaceAction
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
