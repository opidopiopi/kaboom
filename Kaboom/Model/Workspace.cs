using Kaboom.Abstract;
using Kaboom.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Kaboom.Model
{
    public class Workspace : ITreeNode
    {
        private List<Screen> m_screens = new List<Screen>();

        public Workspace(IProvideScreens screenProvider)
        {
            screenProvider.GetScreenBounds().ForEach(bounds => Insert(new Screen(bounds)));
        }

        public void Insert(Screen screen)
        {
            m_screens.Add(screen);
            screen.Insert(new HorizontalArrangement(new Rectangle(0, 0, 1, 1)));
            screen.SetParent(this);
        }

        public void Insert(Window window)
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

        public void Insert(ITreeNode child)
        {
            throw new InvalidChildForThisNode($"Unable to insert child of type {child.GetType()} into Workspace. Valid types are Screen and Window!");
        }

        public bool RemoveAndReturnSuccess(ITreeNode child)
        {
            //we assume that screens will not be removed since it is not currently a use case
            foreach (var screen in m_screens)
            {
                if (screen.RemoveAndReturnSuccess(child))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetParent(ITreeNode parent)
        {
            throw new System.NotImplementedException();
        }

        public ITreeNode GetParent()
        {
            throw new System.NotImplementedException();
        }

        public List<ITreeNode> Children()
        {
            return m_screens.Cast<ITreeNode>().ToList();
        }
    }
}
