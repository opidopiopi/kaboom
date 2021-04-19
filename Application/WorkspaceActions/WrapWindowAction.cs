using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application.WorkspaceActions
{
    public class WrapWindowAction<ArrangementType> : WorkspaceAction
        where ArrangementType : Arrangement, new()
    {
        public WrapWindowAction(Shortcut shortcut, IWorkspace workspace) : base(shortcut, workspace)
        {
        }

        public override void Execute()
        {
            m_workspace.WrapSelectedWindow(new ArrangementType());
        }
    }
}
