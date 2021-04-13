using Kaboom.Domain.ShortcutActions;
using Kaboom.Domain.WindowTree.ArrangementAggregate;

namespace Kaboom.Application.WorkspaceActions
{
    public class WrapWindowAction<ArrangementType> : WorkspaceAction
        where ArrangementType : Arrangement, new()
    {
        public WrapWindowAction(Shortcut shortcut, Workspace workspace) : base(shortcut, workspace)
        {
        }

        public override void Execute(IActionTarget actionTarget)
        {
            m_workspace.WrapSelectedWindow(new ArrangementType());
        }
    }
}
