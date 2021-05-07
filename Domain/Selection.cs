using Kaboom.Domain;
using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree;
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
                SelectWindow(m_windowService.NextWindowInDirection(direction, m_selectedWindow));
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
                foreach (var id in m_arrangementRepository.RootArrangements())
                {
                    var arr = m_arrangementRepository.Find(id);
                    var window = arr.FirstWindow();

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