using Kaboom.Domain;
using Kaboom.Domain.Services;
using Kaboom.Domain.WindowTree;
using Kaboom.Domain.WindowTree.ValueObjects;
using System.Linq;

namespace Kaboom.Application.Services
{
    public class WindowService : IWindowService
    {
        private IArrangementRepository m_arrangements;
        private IRenderService m_renderer;
        private EmptyArrangementRemover m_emptyArrangementRemover = new EmptyArrangementRemover();

        public WindowService(IArrangementRepository arrangements, IRenderService renderer)
        {
            m_arrangements = arrangements;
            m_renderer = renderer;
        }

        public void InsertWindowIntoTree(Window newWindow, ISelection selection)
        {
            Arrangement parent = FindBestParentForWindow(newWindow);

            parent.InsertAsFirst(newWindow);
            UpdateTree();

            if (selection.SelectedWindow == null)
            {
                selection.SelectWindow(newWindow.ID);
            }
        }

        public void RemoveWindowFromTree(EntityID windowID, ISelection selection)
        {
            var parent = m_arrangements.FindParentOf(windowID);
            if (parent == null) return;

            parent.RemoveChild(windowID);
            UpdateTree();
            selection.ClearSelection();
        }

        public void MoveWindow(EntityID windowID, Direction direction)
        {
            var parent = FindParentWithException(windowID);

            MoveWindowRules.MoveWindowByRules(windowID, direction, parent, m_arrangements);
            UpdateTree();
        }

        public EntityID NextWindowInDirection(Direction direction, EntityID currentlySelected)
        {
            var parent = FindParentWithException(currentlySelected);

            return NextWindowRules.NextWindowByRules(direction, currentlySelected, ref parent, m_arrangements);
        }

        public void UnWrapWindowParent(EntityID windowID)
        {
            var parent = m_arrangements.FindParentOf(windowID);
            var superParent = m_arrangements.FindParentOf(parent.ID);

            if (superParent != null)
            {
                superParent.UnWrapChildToSelf(parent.ID);
                UpdateTree();
            }
        }

        public void HightlightWindow(EntityID windowID)
        {
            if (windowID != null)
            {
                m_renderer.HighlightWindow(m_arrangements.FindParentOf(windowID).FindChild(windowID) as Window);
            }
        }

        public void UpdateTree()
        {
            m_arrangements.RootArrangements().ForEach(root =>
            {
                m_emptyArrangementRemover.ExecuteFromRoot(root);
                root.Accept(new TreeUpdate());
            });

            m_renderer.RenderTrees(m_arrangements.RootArrangements());
        }

        private Arrangement FindBestParentForWindow(Window newWindow)
        {
            var parent = m_arrangements.FindThatContainsPoint(newWindow.Bounds.X, newWindow.Bounds.Y);

            if (parent == null)
            {
                parent = m_arrangements.AnyRoot();
            }

            return parent;
        }

        private Arrangement FindParentWithException(EntityID currentlySelected)
        {
            var parent = m_arrangements.FindParentOf(currentlySelected);

            if (parent == null)
            {
                throw new System.Exception("Somehow the currently selected entity has no parent....");
            }

            return parent;
        }
    }
}