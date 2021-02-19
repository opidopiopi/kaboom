using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Model
{
    public class Workspace : ITreeNode, IAcceptWindows, ICanMoveMyChildren
    {
        private List<Screen> m_screens = new List<Screen>();
        private Dictionary<IWindowIdentity, Window> m_windows = new Dictionary<IWindowIdentity, Window>();
        private ISetWindowBounds m_windowBoundsSetter;

#nullable enable
        private Window? m_currentlySelected;
#nullable disable

        public Workspace(IProvideScreens screenProvider, ISetWindowBounds windowBoundsSetter)
        {
            m_windowBoundsSetter = windowBoundsSetter;

            screenProvider.GetScreenBounds().ForEach(CreateAndInsertScreenFromBounds);
        }

        private void CreateAndInsertScreenFromBounds(Rectangle bounds)
        {
            Screen newScreen = new Screen(bounds);

            newScreen.Insert(new HorizontalArrangement());//replace with configurable choice
            newScreen.SetParent(this);

            m_screens.Add(newScreen);
        }

        public void InsertWindow(IWindowIdentity identity, Rectangle bounds)
        {
            if (m_windows.ContainsKey(identity))
            {
                return;
            }

            Window newWindow = new Window(identity, bounds, m_windowBoundsSetter);

            if(m_currentlySelected == null)
            {
                m_currentlySelected = newWindow;
            }

            m_windows[identity] = newWindow;
            InsertWindowIntoTree(newWindow);
        }

        public void RemoveWindow(IWindowIdentity identity)
        {
            if (!m_windows.ContainsKey(identity))
            {
                return;
            }

            if (m_currentlySelected == m_windows[identity])
            {
                if(m_windows.Count > 0)
                {
                    m_currentlySelected = m_windows.Values.First<Window>();
                }
                else
                {
                    m_currentlySelected = null;
                }
            }

            RemoveWindowFromTree(m_windows[identity]);
            m_windows.Remove(identity);
        }

        private void InsertWindowIntoTree(Window window)
        {
            foreach (var screen in m_screens)
            {
                if (screen.Bounds.IsOtherRectangleInside(window.Bounds))
                {
                    screen.Insert(window);
                    return;
                }
            }

            if(m_screens.Count != 0)
            {
                m_screens[0].Insert(window);
            }
            else
            {
                throw new NoScreensInWorkspace($"For some reason there are no screens under this workspace! Something must have been wrong with the ScreenProvider...");
            }
        }

        private void RemoveWindowFromTree(ITreeNode child)
        {
            //we assume that screens will not be removed since it is currently not a use case
            foreach (var screen in m_screens)
            {
                if (screen.RemoveAndReturnSuccess(child))
                {
                    return;
                }
            }
        }

        public List<ITreeNode> Children()
        {
            return m_screens.Cast<ITreeNode>().ToList();
        }

#nullable enable
        public Window? CurrentlySelectedWindow()
        {
            return m_currentlySelected;
        }
#nullable disable


        //not very happy about the fact that we need to implement these methods
        public void Insert(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            throw new System.NotImplementedException();
        }

        public void SetParent(ITreeNode parent)
        {
            throw new System.NotImplementedException();
        }

        public ITreeNode GetParent()
        {
            throw new System.NotImplementedException();
        }

        public void MoveChildUp(ITreeNode child, ITreeNode caller)
        {
            int index = m_screens.IndexOf((Screen) caller);

            m_screens[(index - 1 + m_screens.Count) % m_screens.Count].Insert(child);
        }

        public void MoveChildDown(ITreeNode child, ITreeNode caller)
        {
            int index = m_screens.IndexOf((Screen)caller);

            m_screens[(index + 1 + m_screens.Count) % m_screens.Count].Insert(child);
        }

        public void MoveChildLeft(ITreeNode child, ITreeNode caller)
        {
            int index = m_screens.IndexOf((Screen)caller);

            m_screens[(index - 1 + m_screens.Count) % m_screens.Count].Insert(child);
        }

        public void MoveChildRight(ITreeNode child, ITreeNode caller)
        {
            int index = m_screens.IndexOf((Screen)caller);

            m_screens[(index + 1 + m_screens.Count) % m_screens.Count].Insert(child);
        }

        public bool IsLeaf()
        {
            return false;
        }
    }
}
