using Kaboom.Domain;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application.Actions.SelectionActions
{
    public class SelectWindowAction : SelectionAction
    {
        private Direction m_direction;

        public SelectWindowAction(ISelection selection, Direction direction) : base(selection)
        {
            m_direction = direction;
        }

        public override void Execute()
        {
            m_selection.MoveSelection(m_direction);
        }
    }
}
