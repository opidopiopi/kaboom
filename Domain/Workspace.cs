using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application
{
    public class Workspace : IEntity, IWorkspace
    {
        public EntityID ID { get; } = new EntityID();
        public EntityID SelectedWindow { get => m_selectedWindow; }

        private EntityID m_selectedWindow;
        private IArrangementRepository m_arrangementRepository;
        private IWindowService m_windowService;

        public Workspace(IWindowService windowService, IArrangementRepository arrangementRepository)
        {
            m_windowService = windowService;
            m_arrangementRepository = arrangementRepository;
        }

        public void InsertWindow(Window window)
        {
            if (m_selectedWindow == null)
            {
                m_selectedWindow = window.ID;
            }

            m_windowService.InsertWindowIntoTree(window);
        }

        public void RemoveWindow(EntityID windowID)
        {
            if (m_selectedWindow != null && m_selectedWindow.Equals(windowID))
            {
                m_selectedWindow = null;
            }

            m_windowService.RemoveWindow(windowID);
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
                m_selectedWindow = m_windowService.NextWindowInDirection(direction, m_selectedWindow);
            }
            else
            {
                m_selectedWindow = m_arrangementRepository.AnyRoot().FirstWindow();
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
                var parent = m_arrangementRepository.FindParentOf(m_selectedWindow);
                m_arrangementRepository.FindParentOf(parent.ID).UnWrapChildToSelf(parent.ID);
            }
        }
    }
}