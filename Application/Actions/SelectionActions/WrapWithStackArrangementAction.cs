using Kaboom.Domain;
using Kaboom.Domain.WindowTree;

namespace Kaboom.Application.Actions.SelectionActions
{
    public class WrapWithStackArrangementAction : SelectionAction
    {
        private IArrangementRepository arrangementRepository;

        public WrapWithStackArrangementAction(ISelection selection, IArrangementRepository arrangementRepository) : base(selection)
        {
            this.arrangementRepository = arrangementRepository;
        }

        public override void Execute()
        {
            m_selection.WrapSelectedWindow(new StackArrangement(m_selection, arrangementRepository));
        }
    }
}