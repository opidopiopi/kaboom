using Kaboom.Domain;
using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.Helpers;
using Kaboom.Domain.WindowTree.ValueObjects;

namespace Kaboom.Application
{
    public class Selection : ISelection
    {
        public EntityID ID { get; } = new EntityID();
        public EntityID SelectedWindow { get => m_selectedWindow; }


        private EntityID m_selectedWindow;
        private IArrangementRepository m_arrangementRepository;
        private IWindowService m_windowService;

        public Selection(IWindowService windowService, IArrangementRepository arrangementRepository)
        {
            m_windowService = windowService;
            m_arrangementRepository = arrangementRepository;
        }

        public void MoveSelectedWindow(Direction direction)
        {
            if (m_selectedWindow != null)
            {
                m_windowService.MoveWindow(m_selectedWindow, direction);
            }
        }

        public void MoveSelection(Direction direction)
        {
            if (m_selectedWindow != null)
            {
                var result = m_windowService.NextWindowInDirection(direction, m_selectedWindow);
                if (result != null) SelectWindow(result);
            }
            else
            {
                SelectFirstWindowInTree();
            }
        }

        public void WrapSelectedWindow(Arrangement wrapper)
        {
            if (m_selectedWindow != null)
            {
                m_arrangementRepository.FindParentOf(m_selectedWindow).WrapChildWithNode(m_selectedWindow, wrapper);
            }
        }

        public void UnWrapSelectedWindow()
        {
            if (m_selectedWindow != null)
            {
                m_windowService.UnWrapWindowParent(m_selectedWindow);
            }
        }

        public void SelectWindow(EntityID windowID)
        {
            m_selectedWindow = windowID;
            m_windowService.HightlightWindow(m_selectedWindow);
        }
        public void ClearSelection()
        {
            m_selectedWindow = null;
            m_windowService.HightlightWindow(null);
        }

        private void SelectFirstWindowInTree()
        {
            if (m_selectedWindow == null)
            {
                foreach (var rootID in m_arrangementRepository.RootArrangements())
                {
                    var root = m_arrangementRepository.Find(rootID);
                    var window = WindowFinder.FirstWindowUnderArrangement(root);

                    if (window != null)
                    {
                        SelectWindow(window);
                        return;
                    }
                }
            }
        }
    }
}