using Kaboom.Application.Services;
using Kaboom.Domain.WindowTree.ArrangementAggregate;
using Kaboom.Domain.WindowTree.General;

namespace Kaboom.Application
{
    public class Selection : ISelection
    {
        public EntityID SelectedWindow { get => m_selectedWindow; }

        private EntityID m_selectedWindow;
        private IArrangementRepository m_arrangementRepository;
        private IWindowService m_windowService;

        public Selection(IWindowService windowService, IArrangementRepository arrangementRepository)
        {
            m_windowService = windowService;
            m_arrangementRepository = arrangementRepository;
        }

        public void InsertWindow(Window window)
        {
            m_windowService.InsertWindowIntoTree(window);

            if (m_selectedWindow == null)
            {
                SelectWindow(window.ID);
            }
        }

        public void RemoveWindow(EntityID windowID)
        {
            if (m_selectedWindow != null && m_selectedWindow.Equals(windowID))
            {
                SelectWindow(null);
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
                SelectWindow(m_windowService.NextWindowInDirection(direction, m_selectedWindow));
            }
            else
            {
                foreach(var id in m_arrangementRepository.RootArrangements())
                {
                    var arr = m_arrangementRepository.Find(id);
                    var window = arr.FirstWindow();

                    if(window != null)
                    {
                        SelectWindow(window);
                        return;
                    }
                }
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
    }
}